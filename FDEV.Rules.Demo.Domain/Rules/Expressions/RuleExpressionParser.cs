using FDEV.Rules.Demo.Domain.Rules.Context;
using FDEV.Rules.Demo.Domain.Rules.Utility;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace FDEV.Rules.Demo.Domain.Rules.Expressions
{
    public class RuleExpressionParser
    {
        private readonly ReSettings _reSettings;
        private static IMemoryCache _memoryCache;
        private readonly IDictionary<string, MethodInfo> _methodInfo;

        public RuleExpressionParser(ReSettings reSettings)
        {
            _reSettings = reSettings;
            _memoryCache ??= new MemoryCache(new MemoryCacheOptions {SizeLimit = 1000});
            _methodInfo = new Dictionary<string, MethodInfo>();
            PopulateMethodInfo();
        }

        private void PopulateMethodInfo()
        {
            var dict_add = typeof(Dictionary<string, object>).GetMethod("Add", BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string), typeof(object) }, null);
            _methodInfo.Add("dict_add", dict_add);
        }

        public LambdaExpression Parse(string expression, ParameterExpression[] parameters, Type returnType)
        {
            var config = new ParsingConfig { CustomTypeProvider = new CustomTypeProvider(_reSettings.CustomTypes) };
            return DynamicExpressionParser.ParseLambda(config, false, parameters, returnType, expression);
        }

        public Func<object[], T> Compile<T>(string expression, RuleParameter[] ruleParams)
        {
            var cacheKey = GetCacheKey(expression, ruleParams, typeof(T));
            return _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.SetSize(1);
                var parameterExpressions = GetParameterExpression(ruleParams).ToArray();
                var e = Parse(expression, parameterExpressions, typeof(T));
                var expressionBody = new List<Expression>() { e.Body };
                var wrappedExpression = WrapExpression<T>(expressionBody, parameterExpressions, Array.Empty<ParameterExpression>());
                return wrappedExpression.Compile();
            });
        }

        private Expression<Func<object[], T>> WrapExpression<T>(List<Expression> expressionList, ParameterExpression[] parameters, ParameterExpression[] variables)
        {
            var argExp = Expression.Parameter(typeof(object[]), "args");
            var paramExps = parameters.Select((c, i) =>
            {
                var arg = Expression.ArrayAccess(argExp, Expression.Constant(i));
                return (Expression)Expression.Assign(c, Expression.Convert(arg, c.Type));
            });
            var blockExpSteps = paramExps.Concat(expressionList);
            var blockExp = Expression.Block(parameters.Concat(variables), blockExpSteps);
            return Expression.Lambda<Func<object[], T>>(blockExp, argExp);
        }

        internal Func<object[], Dictionary<string, object>> CompileRuleExpressionParameters(RuleParameter[] ruleParams, RuleExpressionParameter[] ruleExpParams = null)
        {
            ruleExpParams ??= Array.Empty<RuleExpressionParameter>();
            var expression = CreateDictionaryExpression(ruleParams, ruleExpParams);
            return expression.Compile();
        }

        public T Evaluate<T>(string expression, RuleParameter[] ruleParams)
        {
            var func = Compile<T>(expression, ruleParams);
            return func(ruleParams.Select(c => c.Value).ToArray());
        }

        private IEnumerable<Expression> CreateAssignedParameterExpression(RuleExpressionParameter[] ruleExpParams)
        {
            return ruleExpParams.Select((c, _) => Expression.Assign(c.ParameterExpression, c.ValueExpression));
        }

        private IEnumerable<ParameterExpression> GetParameterExpression(RuleParameter[] ruleParams)
        {
            return from ruleParam in ruleParams select ruleParam == null ? throw new ArgumentException($"{nameof(ruleParam)} can't be null.") : ruleParam.ParameterExpression;
        }

        private Expression<Func<object[], Dictionary<string, object>>> CreateDictionaryExpression(RuleParameter[] ruleParams, RuleExpressionParameter[] ruleExpParams)
        {
            var body = new List<Expression>();
            var paramExp = new List<ParameterExpression>();
            var variableExp = new List<ParameterExpression>();
            var variableExpressions = CreateAssignedParameterExpression(ruleExpParams);

            body.AddRange(variableExpressions);

            var dict = Expression.Variable(typeof(Dictionary<string, object>));
            var add = _methodInfo["dict_add"];

            body.Add(Expression.Assign(dict, Expression.New(typeof(Dictionary<string, object>))));
            variableExp.Add(dict);

            for (var i = 0; i < ruleParams.Length; i++)
            {
                paramExp.Add(ruleParams[i].ParameterExpression);
            }
            for (var i = 0; i < ruleExpParams.Length; i++)
            {
                var key = Expression.Constant(ruleExpParams[i].ParameterExpression.Name);
                var value = Expression.Convert(ruleExpParams[i].ParameterExpression, typeof(object));
                variableExp.Add(ruleExpParams[i].ParameterExpression);
                body.Add(Expression.Call(dict, add, key, value));

            }
            // Return value
            body.Add(dict);

            return WrapExpression<Dictionary<string, object>>(body, paramExp.ToArray(), variableExp.ToArray());
        }

        private string GetCacheKey(string expression, RuleParameter[] ruleParameters, Type returnType)
        {
            var paramKey = string.Join("|", ruleParameters.Select(c => c.Name + "_" + c.Type.ToString()));
            var returnTypeKey = returnType?.ToString() ?? "null";
            return $"Expression:{expression}-Params:{paramKey}-ReturnType:{returnTypeKey}";
        }
    }
}

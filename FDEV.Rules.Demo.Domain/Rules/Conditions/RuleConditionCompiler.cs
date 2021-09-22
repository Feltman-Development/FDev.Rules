using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FDEV.Rules.Demo.Domain.Identity;
using FDEV.Rules.Demo.Domain.Rules.Base;
using FDEV.Rules.Demo.Domain.Rules.Expressions;

namespace FDEV.Rules.Demo.Domain.Rules.Conditions
{
    /// <summary>
    /// A consolidated array of methods to compile rules, expressed in any given way
    /// </summary>
    public static class RuleConditionCompiler
    {
        /// <summary>
        /// Compile rule
        /// </summary>
        public static Func<T, bool> CompileCondition<T>(RuleCondition condition)
        {
            var paramUser = Expression.Parameter(typeof(T));
            var expression = BuildExpression<T>(condition, paramUser);
            return Expression.Lambda<Func<T, bool>>(expression, paramUser).Compile();
        }

        public static Func<T, bool> CompileRule<T>(RuleConditionOperator ruleOperator, object value)
        {
            var expressionBuilder = new ExpressionBuilder();
            var parameter = Expression.Parameter(typeof(T));
            var expression = expressionBuilder.BuildExpression<T>(ruleOperator, value, parameter);
            Func<T, bool> func = Expression.Lambda<Func<T, bool>>(expression, parameter).Compile();
            return func;
        }

        public static Func<T, bool> CompileRule<T>(string propertyName, RuleConditionOperator ruleOperator, object value)
        {
            var expressionBuilder = new ExpressionBuilder();
            var parameter = Expression.Parameter(typeof(T));
            var expression = expressionBuilder.BuildExpression<T>(propertyName, ruleOperator, value, parameter);
            Func<T, bool> func = Expression.Lambda<Func<T, bool>>(expression, parameter).Compile();
            return func;
        }

        public static Func<T, bool> CompileCondition<T>(string propertyName, RuleConditionOperator ruleOperator, List<T> values)
        {
            var expressionBuilder = new ExpressionBuilder();
            var parameter = Expression.Parameter(typeof(T));
            Tuple<Expression, ParameterExpression> expression = expressionBuilder.BuildExpression(propertyName, ruleOperator, parameter, values);
            Func<T, bool> compiledExpression = Expression.Lambda<Func<T, bool>>(expression.Item1, expression.Item2).Compile();
            return compiledExpression;
        }

        public static Func<dynamic, bool> CompileCondition(RuleConditionOperator ruleOperator, object value)
        {
            var expressionBuilder = new ExpressionBuilder();
            var parameter = Expression.Parameter(typeof(object));
            var expression = expressionBuilder.BuildExpression<dynamic>(ruleOperator, value, parameter);
            Func<dynamic, bool> func = Expression.Lambda<Func<dynamic, bool>>(expression, parameter).Compile();
            return func;
        }

        public static Func<dynamic, bool> CompileCondition(RuleCondition rule)
        {
            var expressionBuilder = new ExpressionBuilder();
            var parameter = Expression.Parameter(typeof(object));
            var expression = expressionBuilder.BuildExpression<dynamic>(rule.Operator, rule.TargetValue, parameter);
            Func<dynamic, bool> func = Expression.Lambda<Func<dynamic, bool>>(expression, parameter).Compile();
            return func;
        }

        public static Expression BuildExpression<T>(RuleCondition condition, ParameterExpression param)
        {
            var left = Expression.Property(param, condition.TargetName);
            var propertyType = typeof(T).GetProperty(condition.TargetName).PropertyType;

            if (Enum.TryParse(condition.Operator.ToString(), out ExpressionType tBinary))
            {
                var right = Expression.Constant(Convert.ChangeType(condition.TargetValue, propertyType));
                return Expression.MakeBinary(tBinary, left, right);
            }
            else
            {
                var method = propertyType.GetMethod(condition.Operator.ToString());
                var parameterType = method.GetParameters()[0].ParameterType;
                var right = Expression.Constant(Convert.ChangeType(condition.TargetValue, parameterType));
                return Expression.Call(left, method, right);
            }
        }

        public static Func<T, bool>[] CombineConditions<T>(RuleCondition[] conditions)
        {
            List<Func<T, bool>> list = new List<Func<T, bool>>();
            foreach (var condition in conditions)
            {
                if (string.IsNullOrEmpty(condition.TargetName))
                {
                    ExpressionBuilder expressionBuilder = new ExpressionBuilder();
                    var param = Expression.Parameter(typeof(T));
                    Expression expression = expressionBuilder.BuildExpression<T>(condition.Operator, condition.TargetValue, param);
                    Func<T, bool> func = Expression.Lambda<Func<T, bool>>(expression, param).Compile();
                    list.Add(func);
                }
                else
                {
                    ExpressionBuilder expressionBuilder = new ExpressionBuilder();
                    var param = Expression.Parameter(typeof(T));
                    Expression expression = expressionBuilder.BuildExpression<T>(condition.TargetName, condition.Operator, condition.TargetValue, param);
                    Func<T, bool> func = Expression.Lambda<Func<T, bool>>(expression, param).Compile();
                    list.Add(func);
                }
            }
            return list.ToArray();
        }

        public static bool ValidateRules<T>(T value, Func<T, bool>[] rules)
        {
            foreach (var rule in rules)
            {
                if (!rule(value)) return false;
            }
            return true;
        }

        public static bool ValidateRules<T>(T[] values, Func<T, bool>[] rules)
        {
            foreach (var value in values)
            {
                foreach (var rule in rules)
                {
                    if (!rule(value)) return false;
                }
            }
            return true;
        }
    }
}

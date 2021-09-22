using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FDEV.Rules.Demo.Domain.Rules.Results;

namespace FDEV.Rules.Demo.Domain.Rules.Expressions
{
    /// <summary>
    /// Base class for expression builders
    /// </summary>
    internal abstract class RuleExpressionBuilderBase
    {
        /// <summary>
        /// Builds the expression for rule.
        /// </summary>
        //internal abstract RuleFunc<RuleResultTree> BuildDelegateForRule(Rule rule, RuleParameter[] ruleParams);

        internal abstract LambdaExpression Parse(string expression, ParameterExpression[] parameters, Type returnType);

       // internal abstract Func<object[], Dictionary<string, object>> CompileScopedParams(RuleParameter[] ruleParameters, RuleExpressionParameter[] scopedParameters);
    }
}

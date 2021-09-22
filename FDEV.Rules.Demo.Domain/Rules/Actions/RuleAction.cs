using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FDEV.Rules.Demo.Domain.Rules.Conditions;

namespace FDEV.Rules.Demo.Domain.Rules.Actions
{
    /// <summary>
    /// Action to assign to a rule.
    /// </summary>
    public class RuleAction : ActionBase
    {
        /// <inheritdoc />
        public RuleAction() { }

        /// <inheritdoc />
        public RuleAction(string actionName, string memberName, RuleActionOperator actionOperator, string targetValue, object parameter)
        {
            ActionName = actionName;
            MemberName = memberName;
            Operator = actionOperator;
            TargetValue = targetValue;
            Parameter = parameter;
        }

        /// <inheritdoc />
        public override ValueTask<object> Run(ActionContext context, RuleConditionParameter[] ruleParameters)
        {


            return default;
        }

        public string ActionName { get; }
        public string MemberName { get; }
        public RuleActionOperator Operator { get; }
        public Type TargetType { get; }
        public string TargetValue { get; }
        public object Parameter { get; }
    }

    /// <summary>
    /// Get the collection of actions
    /// </summary>
    public class RuleActions : List<RuleAction>
    {

    }

}

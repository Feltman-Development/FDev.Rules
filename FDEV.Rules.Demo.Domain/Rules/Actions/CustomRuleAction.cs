using System.Threading.Tasks;
using FDEV.Rules.Demo.Domain.Rules.Conditions;
using Microsoft.AspNetCore.Mvc;

namespace FDEV.Rules.Demo.Domain.Rules.Actions
{
    public class CustomRuleAction : ActionBase
    {

        public CustomRuleAction(object someInput) //TODO: Change input and method in general
        {
        }

        public override ValueTask<object> Run(ActionContext context, RuleConditionParameter[] ruleParameters)
        {
            var customInput = context.GetContext<string>("customContextInput");
            return default;
            //Add your custom logic here and return a ValueTask
        }
    }
}

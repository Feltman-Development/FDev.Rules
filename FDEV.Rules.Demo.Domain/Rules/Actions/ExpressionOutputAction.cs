using FDEV.Rules.Demo.Domain.Rules.Conditions;
using FDEV.Rules.Demo.Domain.Rules.Expressions;
using System.Threading.Tasks;

namespace FDEV.Rules.Demo.Domain.Rules.Actions
{
    public class OutputExpressionAction //: ActionBase
    {
        //private readonly RuleExpressionParser _ruleExpressionParser;

        //public OutputExpressionAction(RuleExpressionParser ruleExpressionParser) => _ruleExpressionParser = ruleExpressionParser;

        //public override ValueTask<object> Run(ActionContext context, RuleConditionParameter[] ruleParameters)
        //{
        //    var expression = context.GetContext<string>("expression");
        //    return new ValueTask<object>(_ruleExpressionParser.Evaluate<object>(expression, ruleParameters));
        //}
    }
}

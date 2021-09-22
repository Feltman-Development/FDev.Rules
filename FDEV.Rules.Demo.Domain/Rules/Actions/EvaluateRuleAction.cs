using FDEV.Rules.Demo.Domain.Rules.Conditions;
using FDEV.Rules.Demo.Domain.Rules.Results;

namespace FDEV.Rules.Demo.Domain.Rules.Actions
{
    public class EvaluateRuleAction //: ActionBase
    {
        //private readonly RulesEngine _ruleEngine;

        //public EvaluateRuleAction(RulesEngine ruleEngine) => _ruleEngine = ruleEngine;

        //internal async override ValueTask<ActionRuleResult> ExecuteAndReturnResultAsync(ActionContext context, RuleConditionParameter[] ruleParameters, bool includeRuleResults = false)
        //{
        //    var innerResult = await base.ExecuteAndReturnResultAsync(context, ruleParameters, includeRuleResults);
        //    var output = innerResult.Output as ActionRuleResult;
        //    List<RuleResultTree> resultList = null;
        //    if (includeRuleResults)
        //    {
        //        resultList = new List<RuleResultTree>(output.Results);
        //        resultList.AddRange(innerResult.Results);
        //    }
        //    return new ActionRuleResult
        //    {
        //        Output = output.Output,
        //        Exception = innerResult.Exception,
        //        Results = resultList
        //    };
        //}

        //public override async ValueTask<object> Run(ActionContext context, RuleConditionParameter[] ruleParameters)
        //{
        //    var workflowName = context.GetContext<string>("workflowName");
        //    var ruleName = context.GetContext<string>("ruleName");
        //    var ruleResult = await _ruleEngine.ExecuteActionWorkflowAsync(workflowName, ruleName, ruleParameters);
        //    return ruleResult;
        //}
    }
}

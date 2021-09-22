using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FDEV.Rules.Demo.Domain.Rules.Conditions;
using FDEV.Rules.Demo.Domain.Rules.Results;

namespace FDEV.Rules.Demo.Domain.Rules.Actions
{
    public abstract class ActionBase
    {
        internal async virtual ValueTask<RuleResultTree> ExecuteAndReturnResultAsync(ActionContext context, RuleConditionParameter[] ruleParameters, bool includeRuleResults = false)
        {
            var result = new RuleResultTree();
            try
            {
                result.ActionResult.Output = await Run(context, ruleParameters);
            }
            catch (Exception ex)
            {
                result.ActionResult.Exception = new Exception($"Exception while executing {GetType().Name}: {ex.Message}", ex);
            }
            finally
            {
                if (includeRuleResults)
                {
                    result.ChildResults = new List<RuleResultTree>()
                    {
                        context.GetParentRuleResult()
                    };
                }
            }
            return result;
        }

        public abstract ValueTask<object> Run(ActionContext context, RuleConditionParameter[] ruleParameters);
    }

}

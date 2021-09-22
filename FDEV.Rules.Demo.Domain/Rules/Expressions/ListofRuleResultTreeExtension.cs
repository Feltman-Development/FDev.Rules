using System;
using System.Collections.Generic;
using System.Linq;
using FDEV.Rules.Demo.Domain.Rules.Context;
using FDEV.Rules.Demo.Domain.Rules.Results;

namespace FDEV.Rules.Demo.Domain.Rules.Expressions
{
    public static class ListofRuleResultTreeExtension
    {
        public delegate void OnSuccessFunc(string eventName);
        public delegate void OnFailureFunc();

        /// <summary>
        /// Calls the Success Func for the first rule which succeeded among the ruleResults
        /// </summary>
        public static List<RuleResultTree> OnSuccess(this List<RuleResultTree> ruleResultTrees, OnSuccessFunc onSuccessFunc)
        {
            //TODO: handle succes actions
            return null;
            //var successfulRuleResult = ruleResultTrees.Find(ruleResult => ruleResult.IsSuccess);
            //if (successfulRuleResult != null)
            //{
            //    var eventName = successfulRuleResult.Rule.SuccessEvent ?? successfulRuleResult.Rule.RuleName;
            //    onSuccessFunc(eventName);
            //}
            //return ruleResultTrees;
        }

        /// <summary>
        /// Calls the Failure Func if all rules failed in the ruleReults
        /// </summary>
        public static List<RuleResultTree> OnFail(this List<RuleResultTree> ruleResultTrees, OnFailureFunc onFailureFunc)
        {
            //TODO: handle succes actions
            return null;
            //bool allFailure = ruleResultTrees.All(ruleResult => ruleResult.IsSuccess == false);
            //if (allFailure)
            //    onFailureFunc();
            //return ruleResultTrees;
        }

        //internal static RuleFunc<RuleResultTree> ToResultTree(ReSettings reSettings, Rule rule, IEnumerable<RuleResultTree> childRuleResults, Func<object[], bool> isSuccessFunc, string exceptionMessage = "")
        //{
        //    return (inputs) =>
        //    {

        //        var isSuccess = false;
        //        var inputsDict = new Dictionary<string, object>();
        //        try
        //        {
        //            inputsDict = inputs.ToDictionary(c => c.Name, c => c.Value);
        //            isSuccess = isSuccessFunc(inputs.Select(c => c.Value).ToArray());
        //        }
        //        catch (Exception ex)
        //        {
        //            exceptionMessage = GetExceptionMessage($"Error while executing rule : {rule?.Name} - {ex.Message}", reSettings);
        //            HandleRuleException(new RuleException(exceptionMessage, ex), rule, reSettings);
        //            isSuccess = false;
        //        }

        //        return new RuleResultTree { Rule = rule, Inputs = inputsDict, IsSuccess = isSuccess, ChildResults = childRuleResults, ExceptionMessage = exceptionMessage };
        //    };
        //}

        //internal static RuleFunc<RuleResultTree> ToRuleExceptionResult(ReSettings reSettings, Rule rule, Exception ex)
        //{
        //    HandleRuleException(ex, rule, reSettings);
        //    return ToResultTree(reSettings, rule, null, (args) => false, ex.Message);
        //}

        internal static string GetExceptionMessage(string message, ReSettings reSettings)
        {
            return reSettings.IgnoreException ? "" : message;
        }

        internal static void HandleRuleException(Exception ex, Rule rule, ReSettings reSettings)
        {
            ex.Data.Add(nameof(rule.Name), rule.Name);
            //.Data.Add(nameof(rule.RuleExpression), rule.RuleExpression);

            if (!reSettings.EnableExceptionAsErrorMessage)
            {
                throw ex;
            }
        }
    }
}

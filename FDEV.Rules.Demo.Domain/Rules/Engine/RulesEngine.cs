using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FDEV.Rules.Demo.Domain.Rules.Actions;
using FDEV.Rules.Demo.Domain.Rules.Conditions;
using FDEV.Rules.Demo.Domain.Rules.Context;
using FDEV.Rules.Demo.Domain.Rules.Expressions;
using FDEV.Rules.Demo.Domain.Rules.Results;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FDEV.Rules.Demo.Domain.Rules.Engine
{
    public class RulesEngine //TODO: : IRulesEngine
    {
        public RulesEngine(ILogger logger = null, ReSettings reSettings = null)
        {
            //_logger = logger ?? new NullLogger<RulesEngine>();
            //_reSettings = reSettings ?? new ReSettings();
            //_ruleExpressionParser = new RuleExpressionParser(_reSettings);
            //_ruleCompiler = new RuleCompiler(new RuleExpressionBuilderFactory(_reSettings, _ruleExpressionParser), _reSettings, null);
            //_actionFactory = new ActionFactory(GetActionRegistry(_reSettings));
        }

        //private IDictionary<string, Func<ActionBase>> GetActionRegistry(ReSettings reSettings)
        //{
        //    var actionDictionary = GetDefaultActionRegistry();
        //    var customActions = reSettings.CustomActions ?? new Dictionary<string, Func<ActionBase>>();
        //    foreach (var customAction in customActions)
        //    {
        //        actionDictionary.Add(customAction);
        //    }
        //    return actionDictionary;
        //}

        //private readonly ReSettings _reSettings;
        //private readonly RuleExpressionParser _ruleExpressionParser;
        //private readonly RuleConditionCompiler _ruleCompiler;
        //private readonly ActionFactory _actionFactory;
        //private const string ParamParseRegex = "(\\$\\(.*?\\))";

        //#region Public Methods

        ///// <summary>
        ///// This will execute all the rules of the specified workflow
        ///// </summary>
        //public async ValueTask<List<RuleResultTree>> ExecuteAllRulesAsync(string workflowName, params object[] inputs)
        //{
        //    //_logger.LogTrace($"Called {nameof(ExecuteAllRulesAsync)} for workflow {workflowName} and count of input {inputs.Count()}");
        //    var ruleParams = new List<RuleConditionParameter>();

        //    for (var i = 0; i < inputs.Length; i++)
        //    {
        //        var input = inputs[i];
        //        ruleParams.Add(new RuleConditionParameter($"input{i + 1}", input));
        //    }

        //    return await ExecuteAllRulesAsync(workflowName, ruleParams.ToArray());
        //}

        ///// <summary>
        ///// This will execute all the rules of the specified workflow
        ///// </summary>
        //public async ValueTask<List<RuleResultTree>> ExecuteAllRulesAsync(string workflowName, params RuleConditionParameter[] ruleParams)
        //{
        //    var ruleResultList = ValidateWorkflowAndExecuteRule(workflowName, ruleParams);
        //    await ExecuteActionAsync(ruleResultList);
        //    return ruleResultList;
        //}

        //private async ValueTask ExecuteActionAsync(IEnumerable<RuleResultTree> ruleResultList)
        //{
        //    foreach (var ruleResult in ruleResultList)
        //    {
        //        if (ruleResult.ChildResults != null)
        //        {
        //            await ExecuteActionAsync(ruleResult.ChildResults);
        //        }
        //        var actionResult = await ExecuteActionForRuleResult(ruleResult, false);
        //        ruleResult.ActionResult = new ActionResult { Output = actionResult.Output, Exception = actionResult.Exception };
        //    }
        //}

        //public async ValueTask<ActionRuleResult> ExecuteActionWorkflowAsync(string workflowName, string ruleName, RuleConditionParameter[] ruleParameters)
        //{
        //    var compiledRule = CompileRule(workflowName, ruleName, ruleParameters);
        //    var resultTree = compiledRule(ruleParameters);
        //    return await ExecuteActionForRuleResult(resultTree, true);
        //}

        //private async ValueTask<ActionRuleResult> ExecuteActionForRuleResult(RuleResultTree resultTree, bool includeRuleResults = false)
        //{
        //    var ruleActions = resultTree?.Rule?.Actions;
        //    var actionInfo = resultTree?.IsSuccess == true ? ruleActions?.OnSuccess : ruleActions?.OnFailure;

        //    if (actionInfo != null)
        //    {
        //        var action = _actionFactory.Get(actionInfo.Name);
        //        var ruleParameters = resultTree.Inputs.Select(kv => new RuleConditionParameter(kv.Key, kv.Value)).ToArray();
        //        return await action.ExecuteAndReturnResultAsync(new ActionContext(actionInfo.Context, resultTree), ruleParameters, includeRuleResults);
        //    }
        //    else
        //    {
        //        //If there is no action,return output as null and return the result for rule
        //        return new ActionRuleResult
        //        {
        //            Output = null,
        //            Results = includeRuleResults ? new List<RuleResultTree>() { resultTree } : null
        //        };
        //    }
        //}

        //#endregion

        //#region Private Methods

        ///// <summary>
        ///// This will compile the rules and store them to dictionary
        ///// </summary>
        ///// <param name="workflowName">workflow name</param>
        ///// <param name="ruleParams">The rule parameters.</param>
        ///// <returns>
        ///// bool result
        ///// </returns>
        //private bool RegisterRule(string workflowName, params RuleConditionParameter[] ruleParams)
        //{
        //    var compileRulesKey = GetCompiledRulesKey(workflowName, ruleParams);
        //    if (_rulesCache.AreCompiledRulesUpToDate(compileRulesKey, workflowName))
        //    {
        //        return true;
        //    }

        //    var workflow = _rulesCache.GetWorkflow(workflowName);
        //    if (workflow != null)
        //    {
        //        var dictFunc = new Dictionary<string, RuleFunc<RuleResultTree>>();
        //        foreach (var rule in workflow.Rules.Where(c => c.Enabled))
        //        {
        //            dictFunc.Add(rule.RuleName, CompileRule(rule, ruleParams, workflow.GlobalParams?.ToArray()));
        //        }

        //        _rulesCache.AddOrUpdateCompiledRule(compileRulesKey, dictFunc);
        //        _logger.LogTrace($"Rules has been compiled for the {workflowName} workflow and added to dictionary");
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //private RuleFunc<RuleResultTree> CompileRule(string workflowName, string ruleName, RuleConditionParameter[] ruleParameters)
        //{
        //    var workflow = _rulesCache.GetWorkflow(workflowName);
        //    if (workflow == null)
        //    {
        //        throw new ArgumentException($"Workflow `{workflowName}` is not found");
        //    }
        //    var currentRule = workflow.Rules?.SingleOrDefault(c => c.RuleName == ruleName && c.Enabled);
        //    if (currentRule == null)
        //    {
        //        throw new ArgumentException($"Workflow `{workflowName}` does not contain any rule named `{ruleName}`");
        //    }
        //    return CompileRule(currentRule, ruleParameters, workflow.GlobalParams?.ToArray());
        //}

        //private RuleFunc<RuleResultTree> CompileRule(Rule rule, RuleConditionParameter[] ruleParams, ScopedParam[] scopedParams)
        //{
        //    return _ruleCompiler.CompileRule(rule, ruleParams, scopedParams);
        //}



        ///// <summary>
        ///// This will execute the compiled rules 
        ///// </summary>
        ///// <param name="workflowName"></param>
        ///// <param name="ruleParams"></param>
        ///// <returns>list of rule result set</returns>
        //private List<RuleResultTree> ExecuteAllRuleByWorkflow(string workflowName, RuleConditionParameter[] ruleParameters)
        //{
        //    _logger.LogTrace($"Compiled rules found for {workflowName} workflow and executed");

        //    var result = new List<RuleResultTree>();
        //    var compiledRulesCacheKey = GetCompiledRulesKey(workflowName, ruleParameters);
        //    foreach (var compiledRule in _rulesCache.GetCompiledRules(compiledRulesCacheKey)?.Values)
        //    {
        //        var resultTree = compiledRule(ruleParameters);
        //        result.Add(resultTree);
        //    }

        //    FormatErrorMessages(result);
        //    return result;
        //}

        //private string GetCompiledRulesKey(string workflowName, RuleConditionParameter[] ruleParams)
        //{
        //    var key = $"{workflowName}-" + string.Join("-", ruleParams.Select(c => c.Type.Name));
        //    return key;
        //}

        //private IDictionary<string, Func<ActionBase>> GetDefaultActionRegistry()
        //{
        //    return new Dictionary<string, Func<ActionBase>>{
        //        {"OutputExpression",() => new OutputExpressionAction(_ruleExpressionParser) },
        //        {"EvaluateRule", () => new EvaluateRuleAction(this) }
        //    };
        //}

        ///// <summary>
        ///// The result
        ///// </summary>
        //private IEnumerable<RuleResultTree> FormatErrorMessages(IEnumerable<RuleResultTree> ruleResultList)
        //{
        //    if (_reSettings.EnableFormattedErrorMessage)
        //    {
        //        foreach (var ruleResult in ruleResultList?.Where(r => !r.IsSuccess))
        //        {
        //            var errorMessage = ruleResult?.Rule?.ErrorMessage;
        //            if (string.IsNullOrWhiteSpace(ruleResult.ExceptionMessage) && errorMessage != null)
        //            {
        //                var errorParameters = Regex.Matches(errorMessage, ParamParseRegex);

        //                var inputs = ruleResult.Inputs;
        //                foreach (var param in errorParameters)
        //                {
        //                    var paramVal = param?.ToString();
        //                    var property = paramVal?.Substring(2, paramVal.Length - 3);
        //                    if (property?.Split('.')?.Count() > 1)
        //                    {
        //                        var typeName = property?.Split('.')?[0];
        //                        var propertyName = property?.Split('.')?[1];
        //                        errorMessage = UpdateErrorMessage(errorMessage, inputs, property, typeName, propertyName);
        //                    }
        //                    else
        //                    {
        //                        var arrParams = inputs?.Select(c => new { Name = c.Key, c.Value });
        //                        var model = arrParams?.Where(a => string.Equals(a.Name, property))?.FirstOrDefault();
        //                        var value = model?.Value != null ? JsonConvert.SerializeObject(model?.Value) : null;
        //                        errorMessage = errorMessage?.Replace($"$({property})", value ?? $"$({property})");
        //                    }
        //                }
        //                ruleResult.ExceptionMessage = errorMessage;
        //            }

        //        }
        //    }
        //    return ruleResultList;
        //}

        ///// <summary>
        ///// Updates the error message.
        ///// </summary>
        //private static string UpdateErrorMessage(string errorMessage, IDictionary<string, object> inputs, string property, string typeName, string propertyName)
        //{
        //    var arrParams = inputs?.Select(c => new { Name = c.Key, c.Value });
        //    var model = arrParams?.Where(a => string.Equals(a.Name, typeName))?.FirstOrDefault();
        //    if (model != null)
        //    {
        //        var modelJson = JsonConvert.SerializeObject(model?.Value);
        //        var jObj = JObject.Parse(modelJson);
        //        JToken jToken = null;
        //        var val = jObj?.TryGetValue(propertyName, StringComparison.OrdinalIgnoreCase, out jToken);
        //        errorMessage = errorMessage.Replace($"$({property})", jToken != null ? jToken?.ToString() : $"({property})");
        //    }

        //    return errorMessage;
        //}
        //#endregion
    }
}

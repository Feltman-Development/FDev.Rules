// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;
using FDEV.Rules.Demo.Domain.Rules.Results;

namespace FDEV.Rules.Demo.Domain.Rules.Engine
{
    public interface IRulesEngine
    {
        /// <summary>
        /// This will execute all the rules of the specified workflow
        /// </summary>
        /// <param name="workflowName">The name of the workflow with rules to execute against the inputs</param>
        /// <param name="inputs">A variable number of inputs</param>
        /// <returns>List of rule results</returns>
        ValueTask<List<RuleResultTree>> ExecuteAllRulesAsync(string workflowName, params object[] inputs);

        ///// <summary>
        ///// This will execute all the rules of the specified workflow
        ///// </summary>
        ///// <param name="workflowName">The name of the workflow with rules to execute against the inputs</param>
        ///// <param name="ruleParams">A variable number of rule parameters</param>
        ///// <returns>List of rule results</returns>
        //ValueTask<List<RuleResultTree>> ExecuteAllRulesAsync(string workflowName, params RuleParameter[] ruleParams);;
    }
}

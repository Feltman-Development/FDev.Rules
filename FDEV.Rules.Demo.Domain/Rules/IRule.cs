using System.Collections.Generic;
using FDEV.Rules.Demo.Domain.Common;
using FDEV.Rules.Demo.Domain.Rules.Actions;
using FDEV.Rules.Demo.Domain.Rules.Conditions;
using FDEV.Rules.Demo.Domain.Rules.Context;
using FDEV.Rules.Demo.Domain.Rules.Contexts;

namespace FDEV.Rules.Demo.Domain.Rules
{
    /// <summary>
    /// The interface of the rules that can be build and applied in the engine.
    /// </summary>
    public interface IRule : IEntity, IAggregateRoot
    {
        /// <summary>
        /// Get the description of the rule.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Get the properties that constitutes the context for the rule.
        /// </summary>
        RuleContextBase Context { get; set; }

        /// <summary>
        /// Get the conditions to meet for the rule to apply.
        /// </summary>
        RuleConditions Conditions { get; }

        /// <summary>
        /// Get the collection of actions to invoke if conditions are met.
        /// </summary>
        RuleActions ActionsOnSuccess { get; }

        /// <summary>
        /// Get the collection of actions to invoke if conditions are not met.
        /// </summary>
        RuleActions ActionsOnFail { get; }

        /// <summary>
        /// Get errormessage(s) if any of the conditions fail.
        /// - Key = ConditionName.
        /// - Value = ErrorMessage.
        /// </summary>
        Dictionary<string, string> ErrorMessages { get; set; }

        /// <summary>
        /// Get if rule is active
        /// </summary>
        bool IsActive { get; }
    }
}
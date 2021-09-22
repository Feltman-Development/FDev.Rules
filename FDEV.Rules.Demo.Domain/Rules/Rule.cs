using System;
using System.Collections.Generic;
using FDEV.Rules.Demo.Domain.Common;
using FDEV.Rules.Demo.Domain.Rules.Actions;
using FDEV.Rules.Demo.Domain.Rules.Conditions;
using FDEV.Rules.Demo.Domain.Rules.Context;
using FDEV.Rules.Demo.Domain.Rules.Contexts;

namespace FDEV.Rules.Demo.Domain.Rules
{
    /// <summary>
    /// An extremely powerful Rule implementation. There are 3 independant essential parts, that together makes it so versatile:
    ///
    ///  >> 1: The RULE CONTEXT, which is what we compare the conditions below against, which is dynamic and can be anything from
    ///        a simple property, to a collection of entities or a complex object graph (like a shopping order). Beyond the shared
    ///        context, it is also possible to assign individual contexts for each condition and action.
    ///
    ///  >> 2: The rule CONDITIONS. There can be as many as many as you want of, and they are made up of:
    ///     - 2A: A Context. Usually inherited from the parent rule, but can also be assigned its own custom context.
    ///     - 2B: A Target, which is the part of the context we want to compare to.
    ///     - 2C: A TargetParameter, which is the 'what' on the target we want to compare to. And this engine is made to be able
    ///           to use just about anything on or about the target, which is one of the things that makes it so powerful.
    ///     - 2D:  TargetValue', the value we want to compare on. It can be a number, string, type, elements in a collection...
    ///     - 2E: 'ConditionOperator', that defines how we want to compare, and again: there is almost no constraint on our options,
    ///           which means we can compare numbers, types, strings or even write a custom expresssion.
    ///     - 2F: To help understand this rule engine, it can benefit to think of the rule condition being very similar to the whole
    ///           rule in a normal rule engine and the rule as a scenario with multiple conditions, multiple actions and a shared context.
    ///
    ///  >> 3: Two lists of RULE ACTIONS. One to invoke/execute if all conditions are met, and one if a condition has failed. An obvious 
    ///        future extension would be to implement AND/OR on conditions for even more flexibility, but it wasn't required in this version.
    ///     - 3A: A context. Usually inherited from the parent rule, but can also be assigned its own custom context.
    ///     - 3B:
    ///     - 3C:
    ///     - 3D:
    ///     
    ///     - 3A: An Action is made up of almost the same parts as a Condition, . It can of course also be the exactly what we like again, and every action has its own
    ///           context - which means that we can change whatever we like across multiple systems based on the outcome of the conditions.
    ///     - 3B: What an action also has, is the possibity to execute or invoke methods, fire events (that can trigger anything) or call
    ///           an API. We are therefore not limited to affecting our own systems, but can trade stocks, make banktransfers, send out
    ///           press releases/mass email, or just invoke a discount on a shopping order, when the customer buys two of the same item.
    /// </summary>
    public class Rule : Entity, IRule, IAggregateRoot
    {
        /// <summary>
        /// Create instance of rule with the given Context
        /// </summary>
        /// <param name="context"></param>
        public Rule(RuleContextBase  context)
        {
            Context = context;
        }

        /// <inheritdoc />
        public string Description { get; set; }

        /// <inheritdoc />
        public RuleContextBase Context { get; set; }

        /// <inheritdoc />
        public RuleConditions Conditions { get; }

        /// <inheritdoc />
        public RuleActions ActionsOnSuccess { get; }

        /// <inheritdoc />
        public RuleActions ActionsOnFail { get; }

        /// <inheritdoc />
        public Dictionary<string, string> ErrorMessages { get; set; }

        /// <summary>
        /// Gets or sets the status of the rule.
        /// </summary>
        public RuleStatus RuleStatus { get; set; } = RuleStatus.Active;

        /// <inheritdoc />
        public DateTime DeletedAt { get; set; }

        /// <inheritdoc />
        public bool IsActive { get; }

        public delegate void RuleEvent(Rule sender, RuleEventArgs eventArgs);
    }


    public class RuleEventArgs : EventArgs
    {
        public RuleEventType RuleEventType { get; set; }

        public object RuleEventValue { get; set; }
        public string Message { get; set; }
    }

    public enum RuleEventType
    {
        Unknown,
        BeginRule,
        BeginEvaluation,
        ProgressEvaluation,
        EndEvaluation,
        ErrorEvaluation,
        BeginAction,
        ProgressAction,
        EndAction,
        ErrorAction,
        EndRule
    }
}
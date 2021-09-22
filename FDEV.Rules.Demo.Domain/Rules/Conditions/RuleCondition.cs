using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FDEV.Rules.Demo.Domain.Rules.Context;
using FDEV.Rules.Demo.Domain.Rules.Base;
using FDEV.Rules.Demo.Domain.Common;
using FDEV.Rules.Demo.Domain.Rules.Contexts;

namespace FDEV.Rules.Demo.Domain.Rules.Conditions
{
    /// <summary>
    /// Condition to assign to a rule.
    /// This condition class is of the very complex type, allowing for many different scenarios to evaluate on.
    /// And to check multiple dimensions, just add another rule to the RuleContions collection.
    /// - Like that the target needs to be of the type 'Product', the type property SKU = "A", and there needs to be 4 of them to fully meet the conditions.
    /// </summary>
    public class RuleCondition : Entity
    {
        #region Constructor and Factory Methods

        public static RuleCondition Create(IRule rule)
        {
            var condition = new RuleCondition(rule);


            //ConditionName = conditionName;
            //TargetName = targetName;   
            //ConditionOperator = ruleOperator;
            //TargetType = targetType;
            //TargetValue = targetValue;

            return condition;
        }

        public RuleCondition(IRule rule)
        {
            Rule = rule;
        }

        ///// <summary>
        ///// Instantíate a RuleCondition, the very heart of a rule, which is also why some
        ///// </summary>
        //public RuleCondition(IRule rule, string conditionName, string targetName, RuleConditionOperator ruleOperator, Type targetType, object targetGoal, object targetValue, int targetQuantity)
        //{
        //    Rule = rule;
        //    RuleScenario = ;
        //    ConditionName = conditionName;
            
        //    TargetName = targetName;  
        //    //ConditionOperator = ruleOperator;
        //    //TargetType = targetType;
        //    //TargetValue = targetValue;
        //}

        #endregion Constructor and Factory Methods

        /// <summary>
        /// Get the rule the condition applies to.
        /// </summary>
        public IRule Rule { get; }

        /// <summary>
        /// Get the context the condition applies to.
        /// TODO: In next phase there will be a custom context property that will have priority over the shared context.
        /// </summary>
        public RuleContextBase RuleContext => Rule.Context; // CustomContext ?? Rule.Context;

        /// <summary>
        /// Description of the condition. For identifying - and could be used in a human readable rule expression.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The operater used when evaluating the condition.
        /// NOTE: Some operators only makes sense with certain targets, but they are all in one enum for now.
        /// </summary>
        public RuleConditionOperator Operator { get; }

        /// <summary>
        /// Get if the target is a type, enum, property, field, collection...
        /// </summary>
        public RuleConditionTargetType TargetType { get; }

        /// <summary>
        /// Get what changes are made to the target/target value post evaluation.
        /// </summary>
        public RuleConditionTargetPostOperation TargetPostOperation { get; }

        /// <summary>
        /// Name of a property, method or type we will evaluate on.
        /// </summary>
        public string TargetName { get; }

        /// <summary>
        /// The requirement specified in the RuleOperator the target needs to meet (the right side of the comparison).
        /// It might be a value, a text, a specific type, inherited from a specific type - or the name of aanother target to compare with.
        /// </summary>
        public string TargetValue { get; }

        /// <summary>
        /// Get the status of the condition
        /// </summary>
        public ConditionStatus Status { get; }
    }

    /// <summary>
    /// A collection of conditions for a rule. By design of current task all rules are required, but can easily be changed into having conditional operators (AND/OR) between them.
    /// </summary>
    public class RuleConditions : List<RuleCondition>
    {

    }
    
    /// <summary>
    /// The possible states of the condition.
    /// </summary>
    public enum ConditionStatus
    {
        Unknown,
        InActive,
        Active,
        Fail,
        Succes
    }
    
    /// <summary>
    /// The type of the target.
    /// </summary>
    public enum RuleConditionTargetType
    {
        Unknown,
        Custom,
        Property,
        Field,
        CollectionItem,
        Method,
        Type,
        Class,
        Enum,
    }

    /// <summary>
    /// Defines how to compare the target in a rule condition
    /// </summary>
    public enum RuleConditionOperator
    {
        /// <summary>
        /// Returns false
        /// </summary>
        Unknown,

        /// <summary>
        /// The comparison operator will be defined in a custom expression.
        /// </summary>
        Custom,

        // Statement Operators
        True,
        False,

        // Value Operators
        Equals,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        FoundIn,
        NotFoundIn,

        // Collection Operators on target as item
        AnyFoundInCollection,
        AllFoundInCollection,
        NotFoundInCollection,
        QuantityEqualsCollection,
        QuantityLessthanCollection,
        QuantityGreaterThanCollection,
        
        // Context Operators
        AnyFoundInContext,
        AllFoundInContext,
        NotFoundInContext,
        QuantityEqualsInContext,
        QuantityLessthanInContext,
        QuantityGreaterThanInContext,

        // String Operators
        StartsWith,
        Contains,
        DoNotContain,
        MatchRegex,
        NullOrEmpty,
        NotNullOrEmpty,

        // Type Operators
        IsType,
        IsClass,
        IsCollection,
        IsInterface,
        IsEnum,
        InheritsFrom,
        IsNull,
        IsNotNull,

        // Customer Operator - For Shopping Extension
        CustomerIsEmployee,
        CustomerIsManager,
        CustomerIsLoyaltyMemeber,
        
        // Product Operator - For Shopping Extension
        ProductQuantity,
        ProductBundle,
        ProductBuyOneGetOne,
        ProductBuyTwoGetThree,

        // Order Operator - For Shopping Extension
        NumberOfItemsInOrder,
        OrderTotal,
    }

    /// <summary>
    /// A post evaluation action on the target. For example the count of the target could be decremented with the amount "used" for succes, so that we can only
    /// use a value or count for as many times the initial value allows.
    /// </summary>
    public enum RuleConditionTargetPostOperation
    {
        /// <summary>
        /// Nothing is changed.
        /// </summary>
        Nothing, 

        /// <summary>
        /// After a succes evaluation, the target value is subtracted ("used"), and the remainder is used for further evaluations.
        /// </summary>
        ValueOrCountDecrement,
        
        /// <summary>
        /// After a succes evaluation, the target value is added to, and the new value is used for further evaluations.
        /// </summary>
        ValueOrCountIncrement,

        /// <summary>
        /// After a succes evaluation, the target value is to the miminum number for the type, and the remainder is used for further evaluations.
        /// </summary>
        ValueOrCountSetToMinimum,
        
        /// <summary>
        /// After a succes evaluation, the target value set to the maximum number for the type, and the new value is used for further evaluations.
        /// </summary>
        ValueOrCountSetToMaximum,
        
        /// <summary>
        /// After a succes evaluation, the value or count of target is set to zero for further evaluations.
        /// </summary>
        ValueOrCountSetToZero,

        /// <summary>
        /// After a succes evaluation, the value of target is set to null (if possible) for further evaluations.
        /// </summary>
        ValueSetToNull,

        /// <summary>
        /// After a succes evaluation, the value of target is set to default for its type (if possible) for further evaluations.
        /// </summary>
        ValueSetToDefault,
    }
}

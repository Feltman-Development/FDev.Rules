using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FDEV.Rules.Demo.Core.Utility;

namespace FDEV.Rules.Demo.Domain.Rules.Conditions
{
    [ExcludeFromCodeCoverage]
    public class RuleConditionParameter
    {
        public RuleConditionParameter(string name, object value)
        {
            Value = DynamicUtility.GetTypedObject(value);
            Init(name, Value?.GetType());
        }

        internal RuleConditionParameter(string name, Type type) => Init(name, type);

        public Type Type { get; private set; }

        public string Name { get; private set; }

        public object Value { get; private set; }

        public ParameterExpression ParameterExpression { get; private set; }

        private void Init(string name, Type type)
        {
            Name = name;
            Type = type ?? typeof(object);
            ParameterExpression = Expression.Parameter(Type, Name);
        }

    }
}

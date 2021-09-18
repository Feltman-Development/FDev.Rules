using System;

namespace FSites.Core.Domain.Dynamic
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
    internal class TriggerOnChangeAttribute : Attribute
    {
        public TriggerOnChangeAttribute(string triggerPropertyName) => TriggerPropertyName = triggerPropertyName;

        public string TriggerPropertyName { get; private set; }

        public bool VerifyStaticExistence { get; set;}
    }
}
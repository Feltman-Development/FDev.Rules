using System;

namespace FDEV.Rules.Demo.Domain.Common.Dynamic
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
    internal class TriggerOnChangeAttribute : Attribute
    {
        public TriggerOnChangeAttribute(string triggerPropertyName) => TriggerPropertyName = triggerPropertyName;

        public string TriggerPropertyName { get; private set; }

        public bool VerifyStaticExistence { get; set;}
    }
}
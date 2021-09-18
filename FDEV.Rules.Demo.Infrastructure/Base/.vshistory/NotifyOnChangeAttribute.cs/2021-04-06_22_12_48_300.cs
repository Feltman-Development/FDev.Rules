using System;

namespace FSites.Core.Domain.Dynamic
{
    /// <inheritdoc />
    /// <summary>
    /// Assign attribute to a property, method or command and declare it's dependency on another property.
    /// Made for use in a MVVM design, but isn't limited to that :-)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
    internal class NotifyOnChangeAttribute : Attribute
    {
        public NotifyOnChangeAttribute(string triggerPropertyName) => TriggerPropertyName = triggerPropertyName;

        public string TriggerPropertyName { get; }

        public bool VerifyStaticExistence { get; set;}
    }
}
using System;
using System.Collections.Generic;
using FDEV.Rules.Demo.Domain.Common;

namespace FDEV.Rules.Demo.Domain.Rules.Contexts
{
    /// <summary>
    /// The abstract base class for rule contexts, which - for the case of naming - is basically just inheriting/relaying the class <see cref="DynamicEntity"/>,
    /// that itself a composition of <see cref="ExtendableExpandoObject"/> and <see cref="IEntity"/>. The RuleContext implements <see cref="IAggregateRoot"/>.
    /// The dynamic classes are rather complex, but thoroughly documented and good reads if you want to know what possibilities it offers and/or how dynamics work.
    /// </summary>
    public abstract class RuleContextBase : DynamicEntity, IAggregateRoot
    {
        public RuleContextBase() : base(null, null)
        {
        }

        public RuleContextBase(object instance = null, Dictionary<string, object> properties = null) : base(instance, properties)
        {
        }

        /// <inheritdoc />
        public DateTime DeletedAt { get; set; }
    }
}
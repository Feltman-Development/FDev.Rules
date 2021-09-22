using System;
using System.Collections.Generic;

namespace FDEV.Rules.Demo.Domain.Common
{
    /// <summary>
    /// Class that provides extensible properties and methods to an existing object when cast to dynamic.
    /// This dynamic object stores 'extra' properties in a dictionary or checks the actual properties of the instance passed via constructor.
    /// This class can be subclassed to extend an existing type or you can pass in an instance to be extended on.
    ///
    /// Properties (both dynamic and strongly typed) can be accessed through an indexer, and you can both inherit and inject a type to extend on, practically giving
    /// you the possibility of multiple inheritance - something C# in theory doesn't offer - and work with both properties that are manually typed, inherited,
    /// injected with an instance, inserted into a dictonary and/or truly dynamically added to the inner ExpandoObject. Talk about limitless extensibility! :-)
    ///
    /// The type allows you three ways to access its properties:
    /// - Directly: Any explicitly declared properties are accessible
    /// - Dynamic: Dynamic cast allows access to dictionary and native properties/methods
    /// - Dictionary: Any of the extended properties are accessible via IDictionary interface
    /// </summary>
    /// <remarks> 
    /// For more documentation on the dynamic part <seealso cref="ExtendableExpandoObject"/>.
    /// </remarks>    
    public abstract class DynamicEntity : ExtendableExpandoObject, IEntity //TODO: Maybe do all implementation here and enherit Entity?
    {
        //TODO: Make it [JsonSerializeable or something]

        /// <summary>
        /// 1) Instantiates the class with its internal dictionary and any properties assigned/implemented at design time.
        ///    You can still expand the class, both design time and runtime - and even inherit the class.
        /// 2) Allows passing in an existing instance variable that appends/extends to what is already mentioned above.
        ///    If nothing is passed in as the dynamic, this class itself will initialized and used.
        ///
        /// - And from the outside, it appears as one consolidated type.
        /// </summary>
        protected DynamicEntity(object instance = null, Dictionary<string, object> properties = null) : base(instance, properties)
        {
        }

        #region Implementation of IEntity

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public DateTime AddedAt { get; set; }

        /// <inheritdoc />
        public DateTime LastModifiedAt { get; set; }

        /// <inheritdoc />
        public int Version { get; set; }

        #endregion Implementation of IEntity
    }
}
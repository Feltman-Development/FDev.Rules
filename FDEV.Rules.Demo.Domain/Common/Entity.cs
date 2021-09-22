using System;
using System.ComponentModel.DataAnnotations;
using FDEV.Rules.Demo.Core.Utility;

namespace FDEV.Rules.Demo.Domain.Common
{
    /// <summary>
    /// Base class for all entities. To be able to save directly on data context, the entity must implement IAggregateRoot.
    /// </summary>
    public abstract class Entity : IEntity
    {
        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public DateTime AddedAt { get; set; }

        /// <inheritdoc />
        public DateTime LastModifiedAt { get; set; }

        /// <inheritdoc />
        public int Version { get; set; }
    }
}

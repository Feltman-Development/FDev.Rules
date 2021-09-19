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
        protected Entity() => Uid = GuidUtility.NewSequentialGuid();

        [Key]
        /// <inheritdoc />
        public Guid Uid { get; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public DateTime AddedAt { get; set; }

        /// <inheritdoc />
        public DateTime LastModifiedAt { get; set; }

        /// <inheritdoc />
        public int Version { get; set; }

        /// <inheritdoc />
        public bool IsPersisted => !IsTransient(this);
        private static bool IsTransient(IEntity obj) => obj != null && Equals(obj.Uid, default);
    }
}

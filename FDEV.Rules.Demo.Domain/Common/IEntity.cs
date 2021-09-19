using System;

namespace FDEV.Rules.Demo.Domain.Common
{
    /// <summary>
    /// Interface with the properties needed to setup persistence.
    /// Note that some values have default implementation on the interface.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Get the unique Id for the Entity.
        /// </summary>
        /// <remarks> NOTE: Set in Entity class with a incrementing Guid method. </remarks>
        Guid Uid { get; }

        /// <summary>
        /// DateTime of creation.
        /// </summary>
        /// <remarks> NOTE: Set by intersecting on context changes. </remarks>    
        DateTime AddedAt { get; set; }

        /// <summary>
        /// DateTime of creation.
        /// </summary>
        /// <remarks> NOTE: Set by intersecting on context changes. </remarks>
        DateTime LastModifiedAt { get; set; }

        /// <summary>
        /// Version of the instance (ie. how many times changed in Db).
        /// </summary>
        /// <remarks> NOTE: Set by intersecting on context changes. </remarks>
        int Version { get; set; }

        /// <summary>
        /// Get if the Entity is persisted to database
        /// </summary>
        public bool IsPersisted => !IsTransient(this);
        private static bool IsTransient(IEntity obj) => obj != null && Equals(obj.Uid, default);


        #region Default Implementations

        /// <summary>
        /// Set to suppress PropertyChanged events.
        /// </summary>
        bool SuppressPropertyChanged => false;

        /// <summary>
        /// Set to suppress CollectionChanged events.
        /// </summary>
        bool SuppressCollectionChanged => false;

        #endregion Default Implementations
    }
}
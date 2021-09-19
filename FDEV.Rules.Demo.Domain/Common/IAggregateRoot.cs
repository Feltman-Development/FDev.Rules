using System;

namespace FDEV.Rules.Demo.Domain.Common
{
    /// <summary>
    /// Interface for aggregate root, with properties related to (db-agnostic) persistance.
    /// Basically it is a marker interface, but I have added properties to set if the aggregate can perform CRUD operations (to protect data in an auditable system fx)
    /// </summary>
    /// <remarks>
    /// - A DDD - AND CARSTEN FELTMAN - LAYDOWM ON WHAT AN AGGREATE ROOT IS:
    /// - An root is the boundary for an aggregate (hence the name) of a collection of entities,
    ///   that exits only as part of the aggregate and have no global identity.
    /// - Root Entities have global identity. Entities inside the boundary have local identity, unique only within the Aggregate.
    /// - Nothing outside the Aggregate boundary can hold a reference to anything inside, except to the root Entity.
    /// - The root Entity can hand references to the internal Entities to other objects, but they can only use them transiently (within a single method or block).
    /// - Only Aggregate Roots can be obtained directly with database queries. Everything else must be done through traversal.
    /// - Objects within the Aggregate can hold references to other Aggregate roots.
    /// - A delete operation must remove everything within the Aggregate boundary all at once
    /// </remarks>
    public interface IAggregateRoot : IEntity
    {
        /// <summary>
        /// Gets whether the current aggregate can be saved.
        /// </summary>
        bool CanBeSaved => true;

        /// <summary>
        /// Gets whether the current aggregate can be updated.
        /// </summary>
        bool CanBeUpdated => true;

        /// <summary>
        /// Gets whether the current aggregate can be deleted.
        /// If an aggregate cannot be deleted, it will change state to Modified and a DeletedAt date wil be set instead, when saving changes on context
        /// </summary>
        bool CanBeDeleted => false;

        /// <summary>
        /// Gets a date from where this aggregate no longer is active (soft deleted) and will be excluded from queries.
        /// </summary>
        DateTime DeletedAt { get; internal set; }

        /// <summary>
        /// Get if the entity is soft deleted
        /// </summary>
        bool IsDeleted => DeletedAt != default && DeletedAt < DateTime.UtcNow;
    }
}
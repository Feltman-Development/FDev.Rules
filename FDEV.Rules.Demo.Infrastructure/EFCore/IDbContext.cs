using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using FDEV.Rules.Demo.Domain.Common;

namespace FDEV.Rules.Demo.Infrastructure.EFCore;
{
    /// <summary>
    /// An interface for data context 
    /// </summary>
    public interface IDbContext 
    {
        /// <summary>
        /// The main property. The crux of it all, letting us have access to any entity through this one property :-)
        /// </summary>
        DbSet<TEntity> Entities<TEntity>() where TEntity : class, IEntity;

        #region Queries (read only operations)

        /// <summary>
        /// Get if an entity exist in the database.
        /// </summary>
        bool Contains<TEntity>(TEntity entity) where TEntity : class, IEntity;
        
        /// <summary>
        /// Get if any entities exists that matches the query.
        /// </summary>
        bool Exists<TEntity>(Expression<Func<TEntity, bool>> query) where TEntity : class, IEntity;

        /// <summary>
        /// Get a count of entities in the database that matches the query.
        /// </summary>
        long Count<TEntity>(Expression<Func<TEntity, bool>> query) where TEntity : class, IEntity;

        /// <summary>
        /// Get an entity by its id.
        /// </summary>
        TEntity Get<TEntity>(object id) where TEntity : class, IEntity;

        /// <summary>
        /// Find entities matching query and if you will, express a sorting order.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> query = null, Expression<Func<TEntity, string>> orderBy = null) where TEntity : class, IEntity;

        /// <summary>
        /// Find all entities of the type and if you will, express a sorting order. If the entites are just for showing (not editing),
        /// performance can be improved by not tracking the entities on the context. Just Add '.AsNoTracking()' to the query. 
        /// </summary>
        IEnumerable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, string>> orderBy = null) where TEntity : class, IEntity;

        #endregion Queries (read only operations)


        #region Create, Edit and Update operations

        /// <summary>
        /// Insert entity into database.
        /// </summary>
        void Insert<TEntity>(TEntity entity) where TEntity : class, IEntity;

        /// <summary>
        /// Update entity in database.
        /// </summary>
        void Update<TEntity>(TEntity entity) where TEntity : class, IEntity;

        /// <summary>
        /// Insert or Update entity, without knowing if it is in the database already.
        /// </summary>
        void InsertOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity;

        /// <summary>
        /// Special function to insert many entities at the same time, at a fraction of the time single inserts would take.
        /// </summary>
        void BulkInsert<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity;

        /// <summary>
        /// Delete all entities that match query.
        /// </summary>
        void Delete<TEntity>(Expression<Func<TEntity, bool>> query) where TEntity : class, IEntity;
        
        /// <summary>
        /// Delete entity from database.
        /// </summary>
        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity;

        /// <summary>
        /// Delete entity with the given id from the database.
        /// </summary>
        void Delete<TEntity>(Guid id) where TEntity : class, IEntity;

        #endregion Create, Edit and Update operations
    }
}
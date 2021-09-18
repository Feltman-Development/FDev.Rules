using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FRules.Demo.Engine.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using FRules.Demo.Engine.Infrastructure.EFCoreSQLite;

namespace FRules.Demo.Engine.Infrastructure.EFCore
{
    /// <summary>
    /// An implementation of most members of the generic 'IDbContext' and 'IDbContextExecutables', so that when inheriting this for a 
    /// concrete DBContext type, almost no code will have to be written unless you have very specific requirements.
    /// </summary>
    public abstract class DbContextBaseImpl : DbContext, IDbContext, IDbContextExecutables
    {
        /// <summary>
        /// Constructor of the abstract DbContextImplementation.
        /// </summary>
        protected DbContextBaseImpl() { }

        /// <inheritdoc />
        public DbSet<TEntity> Entities<TEntity>() where TEntity : class, IEntity => Set<TEntity>();


        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(IDbContextExecutables).Assembly);
            InvokeOnModelCreatingForAllEntityClasses(builder);
            builder.ApplyBaseEntityConfiguration();
        }

        private static void InvokeOnModelCreatingForAllEntityClasses(ModelBuilder builder)
        {
            var types = Assembly.GetAssembly(typeof(Entity)).GetTypes().
                        Where(s => s.GetInterfaces().Any(x => x == typeof(IEntity) &&
                        s.IsClass && !s.IsAbstract && s.IsPublic));

            foreach (var type in types)
            {
                var onModelCreatingMethod = type.GetMethods().FirstOrDefault(x => x.Name == "OnModelCreating" && x.IsStatic);
                onModelCreatingMethod?.Invoke(type, new object[] { builder });
                if (type.BaseType == null || type.BaseType != typeof(Entity)) continue;

                var baseOnModelCreatingMethod = type.BaseType.GetMethods().FirstOrDefault(x => x.Name == "OnModelCreating" && x.IsStatic);
                if (baseOnModelCreatingMethod == null) continue;

                var baseOnModelCreatingGenericMethod = baseOnModelCreatingMethod.MakeGenericMethod(type);
                baseOnModelCreatingGenericMethod.Invoke(typeof(Entity), new object[] { builder });
            }
        }

        #region Implemntation of IDbContext

        public bool Contains<TEntity>(TEntity entity) where TEntity : class, IEntity => Set<TEntity>().Contains(entity);

        public bool Exists<TEntity>(Expression<Func<TEntity, bool>> query) where TEntity : class, IEntity => Set<TEntity>().Any(query);

        public long Count<TEntity>(Expression<Func<TEntity, bool>> query) where TEntity : class, IEntity => Set<TEntity>().Count(query);

        public TEntity Get<TEntity>(object id) where TEntity : class, IEntity => Set<TEntity>().Find(id);

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> query, Expression<Func<TEntity, string>> orderBy = null) where TEntity : class, IEntity 
            => orderBy == null ? Set<TEntity>().Where(query) : Set<TEntity>().Where(query).OrderBy(orderBy);

        public IEnumerable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, string>> orderBy = null) where TEntity : class, IEntity 
            => orderBy == null ? Set<TEntity>().AsNoTracking() : Set<TEntity>().AsNoTracking().OrderBy(orderBy);

        public new void Update<TEntity>(TEntity entity) where TEntity : class, IEntity => Set<TEntity>().Update(entity);

        public void Insert<TEntity>(TEntity entity) where TEntity : class, IEntity => Set<TEntity>().SingleInsert(entity);

        public void InsertOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            if (Contains(entity))
            {
                Update(entity);
            }
            else 
            {
                Insert(entity);
            }
        }

        public void BulkInsert<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity => Set<TEntity>().BulkInsertAsync(entities);

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> query) where TEntity : class, IEntity => Set<TEntity>().Where(query).DeleteFromQuery();

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity => Set<TEntity>().Remove(entity);

        public void Delete<TEntity>(Guid id) where TEntity : class, IEntity => Set<TEntity>().DeleteByKeyAsync(id);

        #endregion Implemntation of IDbContext


        #region Implementation of IDbContextExecutables
        //TODO: Seeing that the save and execute functionality is a little complicated, it might be an idea to let is have its own class?
        //      'DbContextExecutor' (or something less dramatic), to show that it contains a lot of logic and can have its own reasons to 
        //      change, independent of the context itself - making it comply to S(olid), which is 'Single reason to change' and not 'Single
        //      responsibility' as many will have you think :_)


        public int ExecuteSQL(string sqlString, params object[] parameters) => ExecuteSQL(sqlString);

        public int ExecuteSQL(string sqlString, IEnumerable<object> parameters) => ExecuteSQL(sqlString, parameters);

        public Task<int> ExecuteSQLAsync(string sqlString, params object[] parameters) => ExecuteSQLAsync(sqlString, parameters);

        public Task<int> ExecuteSQLAsync(string sqlString, IEnumerable<object> parameters) => ExecuteSQLAsync(sqlString, parameters);

        /// <summary>
        /// Save changes to database.
        /// </summary>
        public override int SaveChanges() => SaveChanges(true);

        /// <summary>
        /// Save changes to database.
        /// If 'acceptAllChangesOnSuccess' is set to true (default), the changetracker will manually reset the state of tracked
        /// entities, assuming they represent their correct state - after saving to database. If the save to databse fails, the
        /// state of the entities will remain as they were, and another attempt at saving to database can be made.
        /// </summary>
        public override int SaveChanges(bool acceptAllChangesOnSuccess) => SaveChangesAsync(acceptAllChangesOnSuccess).ConfigureAwait(false).GetAwaiter().GetResult();

        /// <summary>
        /// Intercept when saving changes to context and change some properties depending on entry state and entity settings.
        /// </summary>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                var entity = entry.Entity as IEntity;
                var canDeleteAggregateRoot = (entity as IAggregateRoot)?.CanBeDeleted;

                if (entry.State == EntityState.Added)
                {
                    entity.AddedAt = DateTime.UtcNow;
                    entity.Version = 1;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.LastModifiedAt = DateTime.UtcNow;
                    entity.Version++;
                }
                else if (entry.State == EntityState.Deleted && canDeleteAggregateRoot == true)
                {
                    entity.LastModifiedAt = DateTime.UtcNow;
                    entity.Version++;

                }
                else if (entry.State == EntityState.Deleted && canDeleteAggregateRoot == false)
                {
                    // If aggregate is set to CanBeDeleted = false, then a 'soft' delete is done, by adding a DeletedAt date, preventing entity from being fetched by use of filter
                    entry.State = EntityState.Modified;
                    entity.LastModifiedAt = DateTime.UtcNow;
                    entity.Version++;
                    (entity as IAggregateRoot).DeletedAt = DateTime.UtcNow;
                }
            }
            return await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Intercept when saving changes to context and change some properties depending on entry state and entity settings.
        /// If 'acceptAllChangesOnSuccess' is set to true (default), the changetracker will manually reset the state of tracked
        /// entities, assuming they represent their correct state - after saving to database. If the save to databse fails, the
        /// state of the entities will remain as they were, and another attempt at saving to database can be made.
        /// </summary>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = SaveChangesAsync(cancellationToken);
                ChangeTracker.AcceptAllChanges();
                return result;
            }
            catch (Exception)
            {
                //TODO: Handle exception with rolback/retry?
                throw;
            }
        }
    }

    #endregion Implementation of IDbContextExecutables
}
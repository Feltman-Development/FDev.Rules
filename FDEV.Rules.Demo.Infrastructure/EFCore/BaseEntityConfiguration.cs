using System;
using System.Linq;
using System.Reflection;
using FRules.Demo.Engine.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace FRules.Demo.Engine.Infrastructure.EFCore
{
    /// <summary>
    /// Applies common configuration to all base entities
    /// </summary>
    public static class BaseEntityConfiguration
    {
        /// <summary>
        /// Creates a delegate for the configure method below and applies it to entities that match the same constraints as the generic constraint on our data context.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder ApplyBaseEntityConfiguration(this ModelBuilder modelBuilder)
        {
            var method = typeof(BaseEntityConfiguration).GetTypeInfo().DeclaredMethods.Single(m => m.Name == nameof(Configure));
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.IsBaseEntity(out var T))
                    method.MakeGenericMethod(entityType.ClrType, T).Invoke(null, new[] { modelBuilder });
            }
            return modelBuilder;
        }

        /// <summary>
        /// Configuration being applied to all entities
        /// </summary>
        static void Configure<TEntity>(ModelBuilder modelBuilder) where TEntity : class, IEntity
        {
            modelBuilder.Entity<TEntity>(builder =>
            {
                builder.HasKey(e => e.Uid);
                builder.Ignore(x => x.IsPersisted);
                builder.HasQueryFilter(x => (x as IAggregateRoot).DeletedAt < DateTime.UtcNow);
            });
        }

        private static bool IsBaseEntity(this Type type, out Type T)
        {
            for (var baseType = type.BaseType; baseType != null; baseType = baseType.BaseType)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(Entity))
                {
                    T = baseType.GetGenericArguments()[0];
                    return true;
                }
            }
            T = null;
            return false;
        }
    }
}
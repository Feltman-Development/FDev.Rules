using Microsoft.EntityFrameworkCore;
using FRules.Demo.Engine.Domain;
using FRules.Demo.Engine.Infrastructure.EFCoreSQLite;

namespace FRules.Demo.Engine.Infrastructure.EFCore
{
    public class FDevRulesSqlDbContext : DbContextBaseImpl, IDbContext, IDbContextExecutables
    {
        public FDevRulesSqlDbContext()
        {
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DevData.ConnectionString_FDRDEV);
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            
        }
       
        #region Repositories

        //public DbSet<Rule> Rules { get; private set; }

        public DbSet<Order> Orders { get; private set; }

        public DbSet<Customer> Customers { get; private set; }

        public DbSet<Employee> Employees { get; private set; }

        public DbSet<Product> Products { get; private set; }

        public DbSet<Promotion> Promotions { get; private set; }

        #endregion Repositories
    }
}
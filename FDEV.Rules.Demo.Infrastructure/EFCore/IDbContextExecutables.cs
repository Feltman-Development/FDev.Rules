using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FRules.Demo.Engine.Infrastructure.EFCoreSQLite
{
    /// <summary>
    /// Interface with executable functions for the dat context.
    /// </summary>
    public interface IDbContextExecutables
    {
        Task<int> ExecuteSQLAsync(string sqlString, params object[] parameters);

        int ExecuteSQL(string sqlString, params object[] parameters);

        Task<int> ExecuteSQLAsync(string sqlString, IEnumerable<object> parameters);

        int ExecuteSQL(string sqlString, IEnumerable<object> parameters);


        int SaveChanges();

        int SaveChanges(bool acceptAllChangesOnSuccess);

        Task<int> SaveChangesAsync(CancellationToken cancelToken);

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancelToken);
    }
}

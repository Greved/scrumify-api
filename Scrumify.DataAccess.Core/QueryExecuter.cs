using System;
using System.Data;
using System.Threading.Tasks;

namespace Scrumify.DataAccess.Core
{
    public class QueryExecuter
    {
        private readonly IDbConnectionStringProvider dbConnectionStringProvider;

        public QueryExecuter(IDbConnectionStringProvider dbConnectionStringProvider)
        {
            this.dbConnectionStringProvider = dbConnectionStringProvider;
        }

        public async Task<TResult> QueryAsync<TResult>(Func<IDbConnection, Task<TResult>> queryFunc)
        {
            using (var connection = DbConnectionHelper.OpenConnection(dbConnectionStringProvider.Get()))
            {
                return await queryFunc(connection).ConfigureAwait(false);
            }
        }

        public async Task QueryAsync(Func<IDbConnection, Task> queryAction)
        {
            using (var connection = DbConnectionHelper.OpenConnection(dbConnectionStringProvider.Get()))
            {
                await queryAction(connection).ConfigureAwait(false);
            }
        }
    }
}
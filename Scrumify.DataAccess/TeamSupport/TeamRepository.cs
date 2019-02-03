using System;
using System.Threading.Tasks;
using Dapper;
using Scrumify.DataAccess.Core;
using Scrumify.Models;
using Serilog;

namespace Scrumify.DataAccess.TeamSupport
{
	public class TeamRepository : ITeamRepository
	{
	    private readonly QueryExecuter queryExecuter;

		public TeamRepository(QueryExecuter queryExecuter)
		{
		    this.queryExecuter = queryExecuter;
		}

		private const string ExistsQuery = "SELECT (CASE " +
		                                   "WHEN EXISTS(SELECT * FROM team where id = @Id) " +
		                                   "THEN TRUE " +
		                                   "ELSE FALSE " +
		                                   "END)";

        public Task<bool> ExistsAsync(Guid teamId)
		{
		    return queryExecuter.QueryAsync(async connection =>
		    {
		        var result = await connection.QuerySingleAsync<bool>(ExistsQuery, new { Id = teamId }).ConfigureAwait(false);
		        Log.Information("Team {TeamId} exists? {Exist}", teamId, result);
		        return result;
            });
        }

		private const string SaveQuery = "INSERT INTO team " +
		                                 "(id, \"name\") " +
		                                 "VALUES " +
		                                 "(@Id, @Name)";

        public Task SaveAsync(Team team)
		{
			if (team == null)
                throw new ArgumentNullException(nameof(team));

		    return queryExecuter.QueryAsync(async connection =>
		    {
		        var saveResult = await connection.ExecuteAsync(SaveQuery, team).ConfigureAwait(false);
		        Log.Information("Team {TeamId} and {TeamName} inserted with result {SaveResult}", team.Id, team.Name, saveResult);
            });
        }

		private const string DeleteAllQuery = "DELETE FROM team";

        public Task DeleteAllAsync()
		{
		    return queryExecuter.QueryAsync(async connection =>
		    {
		        var result = await connection.ExecuteAsync(DeleteAllQuery).ConfigureAwait(false);
		        Log.Information("All teams were deleted with result {result}", result);
            });
        }
	}
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Scrumify.DataAccess.Core;
using Scrumify.Models;
using Serilog;

namespace Scrumify.DataAccess.UserSupport
{
    public class UserRepository : IUserRepository
    {
        private readonly QueryExecuter queryExecuter;

        public UserRepository(QueryExecuter queryExecuter)
        {
            this.queryExecuter = queryExecuter;
        }

        private const string SaveUserQuery = "INSERT INTO scrumifyuser " +
                                             "(id, outerid, teamid, name) " +
                                             "VALUES " +
                                             "(@Id, @OuterId, @TeamId, @Name)";

        public Task SaveAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return queryExecuter.QueryAsync(async connection =>
            {
                var saveResult = await connection.ExecuteAsync(SaveUserQuery, user);
                Log.Information("User {UserId}, {UserName}, {UserOuterId}, {UserTeamId} inserted with result {SaveResult}",
                    user.Id,
                    user.Name,
                    user.OuterId,
                    user.TeamId,
                    saveResult);
            });
        }

        private const string ReadByIdQuery = @"SELECT * " +
                                             "FROM scrumifyuser " +
                                             "WHERE id = @Id";

        public Task<User> ReadAsync(Guid id)
        {
            return queryExecuter.QueryAsync(async connection =>
            {
                var user = await connection.QuerySingleOrDefaultAsync<User>(ReadByIdQuery, new { Id = id });
                if (user == null)
                {
                    Log.Information("User {UserId} not found by id", id);
                }
                else
                {
                    Log.Information("User {UserId} and {UserName} found by id", user.Id, user.Name);
                }
                return user;
            });
        }

        private const string ReadByOuterIdAndTeamIdQuery = "SELECT u.* " +
                                                  "FROM scrumifyuser u " +
                                                  "WHERE u.outerid = @OuterId AND u.teamid = @TeamId";

        public Task<User> ReadAsync(string outerId, Guid teamId)
        {
            return queryExecuter.QueryAsync(async connection =>
            {
                var user = await connection.QuerySingleOrDefaultAsync<User>(ReadByOuterIdAndTeamIdQuery, new { OuterId = outerId, TeamId = teamId });
                if (user == null)
                {
                    Log.Information("User not found by outer {OuterId} and team id {TeamId}", outerId, teamId);
                }
                else
                {
                    Log.Information("User {UserId} and {UserName} found by outer {OuterId} and team id {TeamId}", user.Id, user.Name, user.OuterId, user.TeamId);
                }
                return user;
            });
        }

        private const string ReadIdByOuterIdAndTeamIdQuery = "SELECT u.id " +
                                                             "FROM scrumifyuser u " +
                                                             "WHERE u.outerid = @OuterId AND u.teamid = @TeamId";

        public Task<Guid> ReadIdAsync(string outerId, Guid teamId)
        {
            return queryExecuter.QueryAsync(async connection =>
            {
                var id = await connection.QuerySingleOrDefaultAsync<Guid>(ReadIdByOuterIdAndTeamIdQuery, new { OuterId = outerId, TeamId = teamId });
                if (id == Guid.Empty)
                {
                    Log.Information("User id not found by outer {OuterId} and team id {TeamId}", outerId, teamId);
                }
                else
                {
                    Log.Information("User id {UserId} found by outer {OuterId} and team id {TeamId}", id, outerId, teamId);
                }
                return id;
            });
        }

        private const string ReadIdsByOuterIdQuery = "SELECT u.Id, u.TeamId " +
                                                             "FROM scrumifyuser u " +
                                                             "WHERE u.outerid = @OuterId";

        public Task<List<UserAndTeamInfo>> ReadInfosByOuterIdAsync(string outerId)
        {
            return queryExecuter.QueryAsync(async connection =>
            {
                var infos = (await connection.QueryAsync<UserAndTeamInfo>(ReadIdsByOuterIdQuery, new { OuterId = outerId })).ToList();
                Log.Information("{IfosLength} infos were found by {OuterId}", infos?.Count, outerId);
                return infos;
            });
        }

        private const string DeleteAllQuery = "DELETE FROM scrumifyuser";

        public Task DeleteAllAsync()
        {
            return queryExecuter.QueryAsync(async connection =>
            {
                var result = await connection.ExecuteAsync(DeleteAllQuery);
                Log.Information("All users were deleted with result {Result}", result);
            });
        }
    }
}
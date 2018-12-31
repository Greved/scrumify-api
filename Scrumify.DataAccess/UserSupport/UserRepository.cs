using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Scrumify.DataAccess.Core;
using Scrumify.Models;
using Serilog;

namespace Scrumify.DataAccess.UserSupport
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionStringProvider dbConnectionStringProvider;

        public UserRepository(IDbConnectionStringProvider dbConnectionStringProvider)
        {
            this.dbConnectionStringProvider = dbConnectionStringProvider;
        }

        private const string SaveUserQuery = "INSERT INTO scrumifyuser " +
                                             "(id, outerid, teamid, name) " +
                                             "VALUES " +
                                             "(@Id, @OuterId, @TeamId, @Name)";

        public void Save(User user)
        {
            var connectionString = dbConnectionStringProvider.Get();
            using (var connection = DbConnectionHelper.OpenConnection(connectionString))
            {
                var saveResult = connection.Execute(SaveUserQuery, user);
                Log.Information("User {UserId}, {UserName}, {UserOuterId}, {UserTeamId} inserted with result {SaveResult}",
                    user.Id,
                    user.Name,
                    user.OuterId,
                    user.TeamId,
                    saveResult);
            }
        }

        private const string ReadByIdQuery = @"SELECT * " +
                                             "FROM scrumifyuser " +
                                             "WHERE id = @Id";

        public User Read(Guid id)
        {
            
            var connectionString = dbConnectionStringProvider.Get();
            using (var connection = DbConnectionHelper.OpenConnection(connectionString))
            {
                var user = connection.QuerySingleOrDefault<User>(ReadByIdQuery, new {Id = id});
                if (user == null)
                {
                    Log.Information("User {UserId} not found by id", id);
                }
                else
                {
                    Log.Information("User {UserId} and {UserName} found by id", user.Id, user.Name);
                }
                return user;
            }
        }

        private const string ReadByOuterIdAndTeamIdQuery = "SELECT u.* " +
                                                  "FROM scrumifyuser u " +
                                                  "WHERE u.outerid = @OuterId AND u.teamid = @TeamId";

        public User Read(string outerId, Guid teamId)
        {
            var connectionString = dbConnectionStringProvider.Get();
            using (var connection = DbConnectionHelper.OpenConnection(connectionString))
            {
                var user = connection.QuerySingleOrDefault<User>(ReadByOuterIdAndTeamIdQuery, new { OuterId = outerId, TeamId = teamId });
                if (user == null)
                {
                    Log.Information("User not found by outer {OuterId} and team id {TeamId}", outerId, teamId);
                }
                else
                {
                    Log.Information("User {UserId} and {UserName} found by outer {OuterId} and team id {TeamId}", user.Id, user.Name, user.OuterId, user.TeamId);
                }
                return user;
            }
        }

        private const string ReadIdByOuterIdAndTeamIdQuery = "SELECT u.id " +
                                                             "FROM scrumifyuser u " +
                                                             "WHERE u.outerid = @OuterId AND u.teamid = @TeamId";

        public Guid ReadId(string outerId, Guid teamId)
        {
            var connectionString = dbConnectionStringProvider.Get();
            using (var connection = DbConnectionHelper.OpenConnection(connectionString))
            {
                var id = connection.QuerySingleOrDefault<Guid>(ReadIdByOuterIdAndTeamIdQuery, new { OuterId = outerId, TeamId = teamId });
                if (id == Guid.Empty)
                {
                    Log.Information("User id not found by outer {OuterId} and team id {TeamId}", outerId, teamId);
                }
                else
                {
                    Log.Information("User id {UserId} found by outer {OuterId} and team id {TeamId}", id, outerId, teamId);
                }
                return id;
            }
        }

        private const string ReadIdsByOuterIdQuery = "SELECT u.Id, u.TeamId " +
                                                             "FROM scrumifyuser u " +
                                                             "WHERE u.outerid = @OuterId";

        public List<UserAndTeamInfo> ReadInfosByOuterId(string outerId)
        {
            var connectionString = dbConnectionStringProvider.Get();
            using (var connection = DbConnectionHelper.OpenConnection(connectionString))
            {
                var infos = connection.Query<UserAndTeamInfo>(ReadIdsByOuterIdQuery, new { OuterId = outerId }).ToList();
                Log.Information("{IfosLength} infos were found by {OuterId}", infos?.Count, outerId);
                return infos;
            }
        }

        private const string DeleteAllQuery = "DELETE FROM scrumifyuser";

        public void DeleteAll()
        {
            var connectionString = dbConnectionStringProvider.Get();
            using (var connection = DbConnectionHelper.OpenConnection(connectionString))
            {
                var result = connection.Execute(DeleteAllQuery);
                Log.Information("All users were deleted with result {Result}", result);
            }
        }
    }
}
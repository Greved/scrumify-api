using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Scrumify.DataAccess.Core;
using Scrumify.Models.ReportItem;
using Serilog;

namespace Scrumify.DataAccess.ReportTaskSupport
{
    public class ReportTaskRepository : IReportTaskRepository
    {
        private readonly QueryExecuter queryExecuter;

        public ReportTaskRepository(QueryExecuter queryExecuter)
        {
            this.queryExecuter = queryExecuter;
        }

        private const string SaveQuery = "INSERT INTO \"report-task\" " +
                                         "(id, userid, teamid, taskdate, ispublic, url, theme, currentstate, problems) " +
                                         "VALUES " +
                                         "(@Id, @UserId, @TeamId, @TaskDate, @IsPublic, @Url, @Theme, @CurrentState, @Problems)";

        public Task SaveAsync(ReportTask reportTask)
        {
            if (reportTask == null)
                throw new ArgumentNullException(nameof(reportTask));
            if (reportTask.DateItemIdentifier == null)
                throw new ArgumentNullException(nameof(ReportTask.DateItemIdentifier));

            return queryExecuter.QueryAsync(async connection =>
            {
                var saveParameters = new ReportTaskStored
                {
                    Id = reportTask.Id,
                    UserId = reportTask.DateItemIdentifier.UserId,
                    TeamId = reportTask.DateItemIdentifier.TeamId,
                    TaskDate = reportTask.DateItemIdentifier.Date,
                    IsPublic = reportTask.IsPublic,
                    Url = reportTask.Url,
                    Theme = reportTask.Theme,
                    CurrentState = reportTask.CurrentState,
                    Problems = reportTask.Problems,
                };
                var saveResult = await connection.ExecuteAsync(SaveQuery, saveParameters).ConfigureAwait(false);
                Log.Information("ReportTask ({Id}, {UserId}, {TeamId}, {TaskDate}, {IsPublic}, {Url}, {Theme}, {CurrentState}, {Problems}) " +
                                "inserted with result {SaveResult}",
                    reportTask.Id,
                    reportTask.DateItemIdentifier.UserId,
                    reportTask.DateItemIdentifier.TeamId,
                    reportTask.DateItemIdentifier.Date,
                    reportTask.IsPublic,
                    reportTask.Url,
                    reportTask.Theme,
                    reportTask.CurrentState,
                    reportTask.Problems,
                    saveResult);
            });
        }

        private const string DeleteByIdQuery = "DELETE " +
                                               "FROM \"report-task\" rt " +
                                               "WHERE rt.id = @Id";

        public Task DeleteAsync(Guid reportTaskId)
        {
            return queryExecuter.QueryAsync(async connection =>
            {
                var result = await connection.ExecuteAsync(DeleteByIdQuery, new { Id = reportTaskId }).ConfigureAwait(false);
                Log.Information("Report task ({reportTaskId}) was deleted with result {result}", reportTaskId, result);
            });
        }

        private const string ReadByDateAndUserQuery = "SELECT rt.* " +
                                                      "FROM \"report-task\" rt " +
                                                      "WHERE rt.teamid = @TeamId " +
                                                      "AND rt.taskdate >= @TaskDateStart " +
                                                      "AND rt.taskdate <= @TaskDateEnd " +
                                                      "AND rt.userid = @UserId " +
                                                      "AND rt.ispublic = @IsPublic";

        public Task<List<ReportTask>> ReadByDateAndUserAsync(ReportDateItemIdentifier dateItemIdentifier,
            bool isPublic)
        {
            if (dateItemIdentifier == null)
                throw new ArgumentNullException(nameof(ReportTask.DateItemIdentifier));

            return queryExecuter.QueryAsync(async connection =>
            {
                var dayPeriod = GetFullDayPeriod(dateItemIdentifier.Date);
                var tasks = (await connection.QueryAsync<ReportTaskStored>(ReadByDateAndUserQuery, new
                    {
                        TeamId = dateItemIdentifier.TeamId,
                        TaskDateStart = dayPeriod.Start,
                        TaskDateEnd = dayPeriod.End,
                        UserId = dateItemIdentifier.UserId,
                        IsPublic = isPublic
                    }).ConfigureAwait(false))
                    .Select(ReportTaskStoredConverter.ToReportTask)
                    .ToList();
                return tasks;
            });
        }

        private const string ReadByDateQuery = "SELECT rt.* " +
                                               "FROM \"report-task\" rt " +
                                               "WHERE rt.teamid = @TeamId " +
                                               "AND rt.taskdate >= @TaskDateStart " +
                                               "AND rt.taskdate <= @TaskDateEnd " +
                                               "AND rt.ispublic = @IsPublic";

        public Task<List<ReportTask>> ReadByDateAsync(Guid teamId, DateTime reportDate, bool isPublic)
        {
            return queryExecuter.QueryAsync(async connection =>
            {
                var dayPeriod = GetFullDayPeriod(reportDate);
                var tasks = (await connection.QueryAsync<ReportTaskStored>(ReadByDateQuery, new
                    {
                        TeamId = teamId,
                        TaskDateStart = dayPeriod.Start,
                        TaskDateEnd = dayPeriod.End,
                        IsPublic = isPublic
                    }).ConfigureAwait(false))
                    .Select(ReportTaskStoredConverter.ToReportTask)
                    .ToList();
                return tasks;
            });
        }

        private static (DateTime Start, DateTime End) GetFullDayPeriod(DateTime date)
        {
            return (date.Date, date.AddDays(1).Date);
        }

        private const string DeleteAllQuery = "DELETE FROM \"report-task\"";

        public Task DeleteAllAsync()
        {
            return queryExecuter.QueryAsync(async connection =>
            {
                var result = await connection.ExecuteAsync(DeleteAllQuery).ConfigureAwait(false);
                Log.Information("All report tasks were deleted with result {result}", result);
            });
        }
    }
}
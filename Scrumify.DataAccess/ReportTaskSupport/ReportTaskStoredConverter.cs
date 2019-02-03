using Scrumify.Models.ReportItem;

namespace Scrumify.DataAccess.ReportTaskSupport
{
    public static class ReportTaskStoredConverter
    {
        public static ReportTask ToReportTask(ReportTaskStored storedTask)
        {
            return new ReportTask
            {
                Id = storedTask.Id,
                CurrentState = storedTask.CurrentState,
                IsPublic = storedTask.IsPublic,
                Problems = storedTask.Problems,
                Theme = storedTask.Theme,
                Url = storedTask.Url,
                DateItemIdentifier = new ReportDateItemIdentifier
                {
                    TeamId = storedTask.TeamId,
                    Date = storedTask.TaskDate,
                    UserId = storedTask.UserId
                }
            };
        }
    }
}
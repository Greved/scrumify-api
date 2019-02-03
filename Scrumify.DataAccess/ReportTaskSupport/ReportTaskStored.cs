using System;

namespace Scrumify.DataAccess.ReportTaskSupport
{
    public class ReportTaskStored
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TeamId { get; set; }
        public DateTime TaskDate { get; set; }
        public bool IsPublic { get; set; }
        public string Url { get; set; }
        public string Theme { get; set; }
        public string CurrentState { get; set; }
        public string Problems { get; set; }
    }
}
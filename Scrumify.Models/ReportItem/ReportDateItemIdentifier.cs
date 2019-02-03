using System;

namespace Scrumify.Models.ReportItem
{
    public class ReportDateItemIdentifier
    {
        public Guid UserId { get; set; }
        public Guid TeamId { get; set; }
        public DateTime Date { get; set; }
    }
}
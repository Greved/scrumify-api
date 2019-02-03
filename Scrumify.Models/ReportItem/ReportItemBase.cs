using System;

namespace Scrumify.Models.ReportItem
{
    public class ReportItemBase
    {
        public Guid Id { get; set; }
        public ReportDateItemIdentifier DateItemIdentifier { get; set; }
        public bool IsPublic { get; set; }
    }
}
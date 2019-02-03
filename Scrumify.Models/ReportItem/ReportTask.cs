namespace Scrumify.Models.ReportItem
{
    public class ReportTask: ReportItemBase
    {
        public string Url { get; set; }
        public string Theme { get; set; }
        public string CurrentState { get; set; }
        public string Problems { get; set; }
    }
}
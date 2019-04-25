namespace Scrumify.Api.Client.Models.ReportDefinition
{
    public class ReportDefinitionItemDto
    {
        public int Order { get; set; }

        public ReportDefinitionQuestionGroupDto Group { get; set; }

        public ReportDefinitionQuestionDto Question { get; set; }
    }
}
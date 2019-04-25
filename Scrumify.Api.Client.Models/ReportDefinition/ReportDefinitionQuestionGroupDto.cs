using System.Collections.Generic;

namespace Scrumify.Api.Client.Models.ReportDefinition
{
    public class ReportDefinitionQuestionGroupDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<ReportDefinitionQuestionDto> Questions { get; set; }
    }
}
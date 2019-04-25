using System.Collections.Generic;

namespace Scrumify.Api.Client.Models.ReportDefinition
{
    public class ReportDefinitionQuestionDto
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public ReportDefinitionQuestionTypeDto Type { get; set; }

        public List<ReportDefinitionQuestionOptionDto> Options { get; set; }
    }
}
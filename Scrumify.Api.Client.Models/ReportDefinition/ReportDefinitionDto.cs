using System.Collections.Generic;

namespace Scrumify.Api.Client.Models.ReportDefinition
{
    public class ReportDefinitionDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<ReportDefinitionItemDto> Items { get; set; }
    }
}
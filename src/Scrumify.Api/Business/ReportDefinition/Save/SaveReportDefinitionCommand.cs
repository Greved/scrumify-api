using MediatR;
using Scrumify.Api.Client.Models.ReportDefinition;

namespace Scrumify.Api.Business.ReportDefinition.Save
{
    public class SaveReportDefinitionCommand: IRequest<string>
    {
        public ReportDefinitionDto ReportDefinition { get; }

        public SaveReportDefinitionCommand(ReportDefinitionDto reportDefinition)
        {
            ReportDefinition = reportDefinition;
        }
    }
}
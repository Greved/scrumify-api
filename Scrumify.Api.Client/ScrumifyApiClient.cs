namespace Scrumify.Api.Client
{
    public class ScrumifyApiClient : IScrumifyApiClient
    {
        public ScrumifyApiClient(IReportDefinitionClient reportDefinition)
        {
            ReportDefinition = reportDefinition;
        }

        public IReportDefinitionClient ReportDefinition { get; }
    }
}
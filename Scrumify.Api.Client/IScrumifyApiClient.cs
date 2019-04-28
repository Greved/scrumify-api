namespace Scrumify.Api.Client
{
    public interface IScrumifyApiClient
    {
        IReportDefinitionClient ReportDefinition { get; }
    }
}
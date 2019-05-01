using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Scrumify.Api.Client.Core;
using Scrumify.Api.Client.Core.CheckResponse;
using Scrumify.Api.Client.Models.ReportDefinition;
using Scrumify.Api.Client.Models.ReportDefinition.List;

namespace Scrumify.Api.Client
{
    public class ReportDefinitionClient : ScrumifyApiClientBase, IReportDefinitionClient
    {
        public ReportDefinitionClient(HttpClient httpClient,
                                      IScumifyApiClientSettings settings,
                                      IScrumifyApiClientResponseChecker responseChecker)
            : base(httpClient, settings, responseChecker)
        {
        }

        public Task<IList<ReportDefinitionListItemDto>> GetAsync(CancellationToken token = default(CancellationToken))
        {
            return GetAsync<IList<ReportDefinitionListItemDto>>($"{Settings.BaseUrl}api/report-definition", token);
        }

        public Task<string> SaveAsync(ReportDefinitionDto reportDefinition, CancellationToken token = default(CancellationToken))
        {
            return PostAsync(reportDefinition, $"{Settings.BaseUrl}api/report-definition",
                x => x, token);
        }

        public Task<long> DeleteAllAsync(CancellationToken token = default(CancellationToken))
        {
            return DeleteAsync($"{Settings.BaseUrl}api/report-definition", (stringData) =>
            {
                long.TryParse(stringData, out var deletedCount);
                return deletedCount;
            }, token);
        }
    }
}
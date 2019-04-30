using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Scrumify.Api.Client.CheckResponse;
using Scrumify.Api.Client.Models.ReportDefinition;
using Scrumify.Api.Client.Models.ReportDefinition.List;

namespace Scrumify.Api.Client
{
    public class ReportDefinitionClient : IReportDefinitionClient
    {
        private readonly HttpClient httpClient;
        //TODO: add logging
        private readonly ILogger<ReportDefinitionClient> logger;
        private readonly IScumifyApiClientSettings settings;
        private readonly IScrumifyApiClientResponseChecker responseChecker;

        public ReportDefinitionClient(HttpClient httpClient,
            ILogger<ReportDefinitionClient> logger,
            IScumifyApiClientSettings settings,
            IScrumifyApiClientResponseChecker responseChecker)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.settings = settings;
            this.responseChecker = responseChecker;
        }

        public async Task<IList<ReportDefinitionListItemDto>> GetAsync()
        {
            var response = await httpClient.GetAsync($"{settings.BaseUrl}api/report-definition");
            await responseChecker.EnsureSuccessAsync(response);
            var data = await response.Content.ReadAsStringAsync();

            var listItems = !string.IsNullOrEmpty(data) ? JsonConvert.DeserializeObject<List<ReportDefinitionListItemDto>>(data) : null;

            return listItems;
        }

        public async Task<string> SaveAsync(ReportDefinitionDto reportDefinition)
        {
            var definitionContent = new StringContent(JsonConvert.SerializeObject(reportDefinition), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{settings.BaseUrl}api/report-definition", definitionContent);
            await responseChecker.EnsureSuccessAsync(response);

            var definitionId = await response.Content.ReadAsStringAsync();
            return definitionId;
        }

        public async Task<long> DeleteAllAsync()
        {
            var response = await httpClient.DeleteAsync($"{settings.BaseUrl}api/report-definition");
            await responseChecker.EnsureSuccessAsync(response);

            var stringDeletedCount = await response.Content.ReadAsStringAsync();
            long.TryParse(stringDeletedCount, out var deletedCount);
            return deletedCount;
        }
    }
}
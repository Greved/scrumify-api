using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        public ReportDefinitionClient(HttpClient httpClient,
            ILogger<ReportDefinitionClient> logger,
            IScumifyApiClientSettings settings)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.settings = settings;
        }

        public async Task<IList<ReportDefinitionListItemDto>> GetAsync()
        {
            var data = await httpClient.GetStringAsync($"{settings.BaseUrl}api/report-definition");

            var listItems = !string.IsNullOrEmpty(data) ? JsonConvert.DeserializeObject<List<ReportDefinitionListItemDto>>(data) : null;

            return listItems;
        }

        public async Task<string> SaveAsync(ReportDefinitionDto reportDefinition)
        {
            var definitionContent = new StringContent(JsonConvert.SerializeObject(reportDefinition), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{settings.BaseUrl}api/report-definition", definitionContent);
            response.EnsureSuccessStatusCode();

            var definitionId = await response.Content.ReadAsStringAsync();
            return definitionId;
        }

        public async Task<long> DeleteAllAsync()
        {
            var response = await httpClient.DeleteAsync($"{settings.BaseUrl}api/report-definition");
            response.EnsureSuccessStatusCode();

            var stringDeletedCount = await response.Content.ReadAsStringAsync();
            long.TryParse(stringDeletedCount, out var deletedCount);
            return deletedCount;
        }
    }
}
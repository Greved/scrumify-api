using System.Collections.Generic;
using System.Threading.Tasks;
using Scrumify.Api.Client.Models.ReportDefinition;
using Scrumify.Api.Client.Models.ReportDefinition.List;

namespace Scrumify.Api.Client
{
    public interface IReportDefinitionClient
    {
        Task<IList<ReportDefinitionListItemDto>> GetAsync();
        Task<string> SaveAsync(ReportDefinitionDto reportDefinition);
        Task<long> DeleteAllAsync();
    }
}
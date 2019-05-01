using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scrumify.Api.Client.Models.ReportDefinition;
using Scrumify.Api.Client.Models.ReportDefinition.List;

namespace Scrumify.Api.Client
{
    public interface IReportDefinitionClient
    {
        Task<IList<ReportDefinitionListItemDto>> GetAsync(CancellationToken token = default(CancellationToken));
        Task<string> SaveAsync(ReportDefinitionDto reportDefinition, CancellationToken token = default(CancellationToken));
        Task<long> DeleteAllAsync(CancellationToken token = default(CancellationToken));
    }
}
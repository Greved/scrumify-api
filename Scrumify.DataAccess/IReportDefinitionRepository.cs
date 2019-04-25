using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scrumify.DataAccess.Models;

namespace Scrumify.DataAccess
{
    public interface IReportDefinitionRepository
    {
        Task<IList<ReportDefinitionListItem>> ReadAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<string> SaveAsync(ReportDefinition definition, CancellationToken cancellationToken = default(CancellationToken));

        Task<ReportDefinition> ReadAsync(string id, CancellationToken cancellationToken = default(CancellationToken));

        Task<long> DeleteAllAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
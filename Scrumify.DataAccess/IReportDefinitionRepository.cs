using System.Threading.Tasks;
using Scrumify.DataAccess.Models;

namespace Scrumify.DataAccess
{
    public interface IReportDefinitionRepository
    {
        Task<string> SaveAsync(ReportDefinition definition);

        Task<ReportDefinition> ReadAsync(string id);

        Task<long> DeleteAllAsync();
    }
}
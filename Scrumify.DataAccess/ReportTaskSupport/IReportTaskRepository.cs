using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scrumify.Models.ReportItem;

namespace Scrumify.DataAccess.ReportTaskSupport
{
    public interface IReportTaskRepository
    {
        Task SaveAsync(ReportTask reportTask);
        Task DeleteAsync(Guid reportTaskId);
        Task<List<ReportTask>> ReadByDateAndUserAsync(ReportDateItemIdentifier dateItemIdentifier, bool isPublic);
        Task<List<ReportTask>> ReadByDateAsync(Guid teamId, DateTime reportDate, bool isPublic);
        Task DeleteAllAsync();
    }
}
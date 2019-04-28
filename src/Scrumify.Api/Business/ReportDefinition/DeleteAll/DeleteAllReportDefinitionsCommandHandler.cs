using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scrumify.DataAccess;

namespace Scrumify.Api.Business.ReportDefinition.DeleteAll
{
    public class DeleteAllReportDefinitionsCommandHandler: IRequestHandler<DeleteAllReportDefinitionsCommand, long>
    {
        private readonly IReportDefinitionRepository repository;

        public DeleteAllReportDefinitionsCommandHandler(IReportDefinitionRepository repository)
        {
            this.repository = repository;
        }

        public Task<long> Handle(DeleteAllReportDefinitionsCommand request, CancellationToken cancellationToken)
        {
            return repository.DeleteAllAsync(cancellationToken);
        }
    }
}
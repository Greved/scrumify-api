using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scrumify.DataAccess;

namespace Scrumify.Api.Business.ReportDefinition.Save
{
    public class SaveReportDefinitionCommandHandler: IRequestHandler<SaveReportDefinitionCommand, string>
    {
        private readonly IReportDefinitionRepository repository;

        public SaveReportDefinitionCommandHandler(IReportDefinitionRepository repository)
        {
            this.repository = repository;
        }

        public async Task<string> Handle(SaveReportDefinitionCommand request, CancellationToken cancellationToken)
        {
            var storedDefinition = ReportDefinitionEntityConverter.ToStored(request.ReportDefinition);
            var definitionId = await repository.SaveAsync(storedDefinition, cancellationToken);
            return definitionId;
        }
    }
}
using DryIoc;
using FluentValidation;
using MediatR;
using Scrumify.Api.Business.ReportDefinition.DeleteAll;
using Scrumify.Api.Business.ReportDefinition.Save;
using Scrumify.Core.DI;

namespace Scrumify.Api.Business.ReportDefinition
{
    public class ReportDefinitionModule: IModule
    {
        public void Load(IRegistrator builder)
        {
            builder.Register(typeof(IRequestHandler<SaveReportDefinitionCommand, string>), typeof(SaveReportDefinitionCommandHandler));
            builder.Register(typeof(IRequest<string>), typeof(SaveReportDefinitionCommand));
            builder.Register(typeof(IValidator<SaveReportDefinitionCommand>), typeof(SaveReportDefinitionCommandValidator));

            builder.Register(typeof(IRequestHandler<DeleteAllReportDefinitionsCommand, long>), typeof(DeleteAllReportDefinitionsCommandHandler));
            builder.Register(typeof(IRequest<long>), typeof(DeleteAllReportDefinitionsCommand));
            builder.Register(typeof(IValidator<DeleteAllReportDefinitionsCommand>), typeof(DeleteAllReportDefinitionsCommandValidator));
        }
    }
}
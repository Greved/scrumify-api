using DryIoc;
using FluentValidation;
using MediatR;
using Scrumify.Api.Business.ReportDefinition.Save;
using Scrumify.Api.Infrastructure.DI;

namespace Scrumify.Api.Business.ReportDefinition
{
    public class ReportDefinitionModule: IModule
    {
        public void Load(IRegistrator builder)
        {
            builder.Register(typeof(IRequestHandler<,>), typeof(SaveReportDefinitionCommandHandler), Reuse.Singleton);
            builder.Register(typeof(IRequest<>), typeof(SaveReportDefinitionCommand), Reuse.Singleton);
            builder.Register(typeof(IValidator<>), typeof(SaveReportDefinitionCommandValidator), Reuse.Singleton);
        }
    }
}
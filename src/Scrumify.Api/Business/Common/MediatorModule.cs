using DryIoc;
using MediatR;
using Scrumify.Api.Business.Common.Behaviors;
using Scrumify.Api.Infrastructure.DI;

namespace Scrumify.Api.Business.Common
{
    public class MediatorModule: IModule
    {
        public void Load(IRegistrator builder)
        {
            builder.RegisterDelegate<ServiceFactory>(r => r.Resolve);
            builder.RegisterMany(new []{typeof(IMediator).GetAssembly()}, type => type.IsInterface, Reuse.Singleton);
            builder.Register(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            builder.Register(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        }
    }
}
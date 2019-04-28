using DryIoc;
using Scrumify.Api.Business.Common;
using Scrumify.Api.Business.ReportDefinition;
using Scrumify.Core.DI;
using Scrumify.DataAccess;
using Scrumify.DataAccess.Mongo;

namespace Scrumify.Api.Infrastructure.DI
{
	public class CompositionRoot
	{
		public CompositionRoot(IContainer container)
		{
            container.Register<IMongoSettings, MongoSettings>(Reuse.Singleton);
            container.Register<IMongoStorage, MongoStorage>(Reuse.Singleton);
            container.Register(typeof(IMongoCollectionProvider<>),
                            typeof(MongoCollectionProvider<>),
                            Reuse.Singleton);
            container.Register<IReportDefinitionRepository, ReportDefinitionRepository>(Reuse.Singleton);

            container.Register<IModule, MediatorModule>();
            container.Register<IModule, ReportDefinitionModule>();

            foreach (var module in container.ResolveMany<IModule>())
                module.Load(container);
        }
    }
}
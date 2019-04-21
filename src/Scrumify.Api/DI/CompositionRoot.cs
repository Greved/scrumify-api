using DryIoc;
using Scrumify.DataAccess;
using Scrumify.DataAccess.Mongo;

namespace Scrumify.Api.DI
{
	public class CompositionRoot
	{
		public CompositionRoot(IRegistrator registrator)
		{
            registrator.Register<IMongoSettings, MongoSettings>(Reuse.Singleton);
            registrator.Register<IMongoStorage, MongoStorage>(Reuse.Singleton);
            registrator.Register(typeof(IMongoCollectionProvider<>),
                            typeof(MongoCollectionProvider<>),
                            Reuse.Singleton);
            registrator.Register<IReportDefinitionRepository, ReportDefinitionRepository>(Reuse.Singleton);
        }
    }
}
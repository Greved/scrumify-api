using DryIoc;
using Scrumify.Api.DataAccess;
using Scrumify.DataAccess.Core;

namespace Scrumify.Api.DI
{
	public class CompositionRoot
	{
		public CompositionRoot(IRegistrator registrator)
		{
			registrator.Register<IDbConnectionStringProvider, DbConnectionStringProvider>();
		}
    }
}
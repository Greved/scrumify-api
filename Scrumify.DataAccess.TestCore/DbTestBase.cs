using FakeItEasy;
using NUnit.Framework;
using Scrumify.DataAccess.Core;

namespace Scrumify.DataAccess.TestCore
{
	public abstract class DbTestBase
	{
		protected IDbConnectionStringProvider DbConnectionStringProvider;

        [OneTimeSetUp]
		public virtual void Setup()
		{
			DbConnectionStringProvider = A.Fake<IDbConnectionStringProvider>();
			A.CallTo(() => DbConnectionStringProvider.Get()).Returns(@"Server=127.0.0.1;Port=5433;Database=scrumify-test;Userid=postgres;Password=postgres;SslMode=Require;");
		}
    }
}
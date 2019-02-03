using FakeItEasy;
using NUnit.Framework;
using Scrumify.DataAccess.Core;
using Serilog;
using Serilog.Events;

namespace Scrumify.DataAccess.TestCore
{
	public abstract class DbTestBase
	{
		protected IDbConnectionStringProvider DbConnectionStringProvider;

        [OneTimeSetUp]
		public virtual void Setup()
		{
		    Log.Logger = new LoggerConfiguration()
		        .MinimumLevel.Debug()
		        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
		        .Enrich.FromLogContext()
		        .WriteTo.Console()
		        .CreateLogger();

            DbConnectionStringProvider = A.Fake<IDbConnectionStringProvider>();
			A.CallTo(() => DbConnectionStringProvider.Get()).Returns(@"Server=127.0.0.1;Port=5433;Database=scrumify-test;Userid=postgres;Password=postgres;SslMode=Require;");
		}
    }
}
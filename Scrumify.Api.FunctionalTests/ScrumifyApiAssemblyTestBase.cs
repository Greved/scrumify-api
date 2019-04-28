using NUnit.Framework;
using Serilog;
using Serilog.Events;

namespace Scrumify.Api.FunctionalTests
{
    [SetUpFixture]
    public class ScrumifyApiAssemblyTestBase
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
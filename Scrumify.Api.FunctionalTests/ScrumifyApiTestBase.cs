using NUnit.Framework;
using Scrumify.Api.TestHost;
using Serilog;

namespace Scrumify.Api.FunctionalTests
{
    [TestFixture]
    public abstract class ScrumifyApiTestBase
    {
        protected ScrumifyApiLocal ApiLocal;

        [OneTimeSetUp]
        public void Setup()
        {
            ApiLocal = ScrumifyApiLocal.Start(Log.Logger);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            ApiLocal.Dispose();
        }
    }
}
using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrumify.Api.Client;
using Serilog;

namespace Scrumify.Api.TestHost
{
    public class ScrumifyApiLocal: IDisposable
    {
        private readonly TestServer server;
        public IScrumifyApiClient Client { get; }

        public ScrumifyApiLocal(TestServer server, IScrumifyApiClient client)
        {
            this.server = server;
            Client = client;
        }

        public static ScrumifyApiLocal Start(ILogger logger)
        {
            var hostBuilder = new WebHostBuilder()
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("appsettings.json", optional: false);
                })
                .UseStartup<ScrumifyApiLocalStartup>()
                .UseSerilog(logger);
            var testServer = new TestServer(hostBuilder);
            var serverHttpClient = testServer.CreateClient();

            var localServiceProvider = ScrumifyApiLocalServiceProviderBuilder.Get(serverHttpClient, logger);
            var apiClient = localServiceProvider.GetService<IScrumifyApiClient>();
            return new ScrumifyApiLocal(testServer, apiClient);
        }

        public void Dispose()
        {
            server?.Dispose();
        }
    }
}
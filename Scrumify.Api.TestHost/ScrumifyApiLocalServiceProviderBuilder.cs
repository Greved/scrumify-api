using System;
using System.Net.Http;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Scrumify.Api.Client;
using Serilog;

namespace Scrumify.Api.TestHost
{
    public static class ScrumifyApiLocalServiceProviderBuilder
    {
        public static IServiceProvider Get(HttpClient serverClient, ILogger serilogLogger)
        {
            var container = new Container();
            container.UseInstance(serverClient);
            var clientSettings = new ScumifyApiClientSettings { BaseUrl = serverClient.BaseAddress.ToString() };
            container.UseInstance<IScumifyApiClientSettings>(clientSettings);

            var clientServiceCollection = new ServiceCollection();
            clientServiceCollection.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true, logger: serilogLogger));
            var serviceProvider = container.WithDependencyInjectionAdapter(clientServiceCollection).ConfigureServiceProvider<ScrumifyApiLocalCompositionRoot>();
            return serviceProvider;
        }
    }
}
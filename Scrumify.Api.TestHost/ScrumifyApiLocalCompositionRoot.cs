using System.Net.Http;
using DryIoc;
using Scrumify.Api.Client.DryIoc;
using Scrumify.Core.DI;

namespace Scrumify.Api.TestHost
{
    public class ScrumifyApiLocalCompositionRoot
    {
        public ScrumifyApiLocalCompositionRoot(IContainer container)
        {
            container.Register<IModule, ScrumifyApiClientModule>();
            foreach (var module in container.ResolveMany<IModule>())
                module.Load(container);
        }
    }
}
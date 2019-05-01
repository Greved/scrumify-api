using DryIoc;
using Scrumify.Api.Client.Core.CheckResponse;
using Scrumify.Core.DI;

namespace Scrumify.Api.Client.DryIoc
{
    public class ScrumifyApiClientModule: IModule
    {
        public void Load(IRegistrator builder)
        {
            builder.Register<IScrumifyApiClient, ScrumifyApiClient>();
            builder.Register<IReportDefinitionClient, ReportDefinitionClient>();
            builder.Register<IScrumifyApiClientResponseChecker, ScrumifyApiClientResponseChecker>();
        }
    }
}
using Microsoft.Extensions.Configuration;

namespace Scrumify.Api.TestHost
{
    public class ScrumifyApiLocalStartup: Startup
    {
        public ScrumifyApiLocalStartup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
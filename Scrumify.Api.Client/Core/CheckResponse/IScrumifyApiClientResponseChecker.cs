using System.Net.Http;
using System.Threading.Tasks;

namespace Scrumify.Api.Client.Core.CheckResponse
{
    public interface IScrumifyApiClientResponseChecker
    {
        Task EnsureSuccessAsync(HttpResponseMessage response);
    }
}
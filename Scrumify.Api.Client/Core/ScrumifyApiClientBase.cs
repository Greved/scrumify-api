using System;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Scrumify.Api.Client.Core.CheckResponse;

namespace Scrumify.Api.Client.Core
{
    public abstract class ScrumifyApiClientBase
    {
        protected readonly IScumifyApiClientSettings Settings;

        private readonly HttpClient httpClient;
        private readonly IScrumifyApiClientResponseChecker responseChecker;

        public ScrumifyApiClientBase(HttpClient httpClient,
            IScumifyApiClientSettings settings,
            IScrumifyApiClientResponseChecker responseChecker)
        {
            this.httpClient = httpClient;
            Settings = settings;
            this.responseChecker = responseChecker;
        }

        public Task<TResponse> PostAsync<TRequestBody, TResponse>(TRequestBody requestBody,
            string requestUrl,
            Func<string, TResponse> convertResponse,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return ExecuteRequestAsync(requestBody, content => httpClient.PostAsync(requestUrl, content, cancellationToken), convertResponse);
        }

        public Task<TResponse> PostAsync<TRequestBody, TResponse>(TRequestBody requestBody,
            string requestUrl,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return ExecuteRequestAsync(requestBody, content => httpClient.PostAsync(requestUrl, content, cancellationToken), JsonConvertResponse<TResponse>);
        }

        public Task PostAsync<TRequestBody>(TRequestBody requestBody,
            string requestUrl,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return ExecuteRequestAsync(requestBody, (content) => httpClient.PostAsync(requestUrl, content, cancellationToken));
        }

        public Task<TResponse> DeleteAsync<TResponse>(string requestUrl,
            Func<string, TResponse> convertResponse,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return ExecuteRequestAsync(() => httpClient.DeleteAsync(requestUrl, cancellationToken), convertResponse);
        }

        public Task<TResponse> DeleteAsync<TResponse>(string requestUrl,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return ExecuteRequestAsync(() => httpClient.DeleteAsync(requestUrl, cancellationToken), JsonConvertResponse<TResponse>);
        }

        public Task<TResponse> GetAsync<TResponse>(string requestUrl,
            Func<string, TResponse> convertResponse,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return ExecuteRequestAsync(() => httpClient.GetAsync(requestUrl, cancellationToken), convertResponse);
        }

        public Task<TResponse> GetAsync<TResponse>(string requestUrl,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return ExecuteRequestAsync(() => httpClient.GetAsync(requestUrl, cancellationToken), JsonConvertResponse<TResponse>);
        }

        private async Task<TResponse> ExecuteRequestAsync<TRequestBody, TResponse>(TRequestBody requestBody, Func<HttpContent, Task<HttpResponseMessage>> sendRequest,
            Func<string, TResponse> convertResponse)
        {
            var jsonContent = GetJsonContent(requestBody);
            var response = await sendRequest(jsonContent);
            await responseChecker.EnsureSuccessAsync(response);
            var data = await response.Content.ReadAsStringAsync();
            var convertedResponse = convertResponse(data);
            return convertedResponse;
        }

        private async Task<TResponse> ExecuteRequestAsync<TResponse>(Func<Task<HttpResponseMessage>> sendRequest,
            Func<string, TResponse> convertResponse)
        {
            var response = await sendRequest();
            await responseChecker.EnsureSuccessAsync(response);
            var data = await response.Content.ReadAsStringAsync();
            var convertedResponse = convertResponse(data);
            return convertedResponse;
        }

        private async Task ExecuteRequestAsync<TRequestBody>(TRequestBody requestBody, Func<HttpContent, Task<HttpResponseMessage>> sendRequest)
        {
            var jsonContent = GetJsonContent(requestBody);
            var response = await sendRequest(jsonContent);
            await responseChecker.EnsureSuccessAsync(response);
        }

        private static StringContent GetJsonContent<TRequestBody>(TRequestBody requestBody)
        {
            return new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        private static TResponse JsonConvertResponse<TResponse>(string stringData)
        {
            return JsonConvert.DeserializeObject<TResponse>(stringData);
        }
    }
}
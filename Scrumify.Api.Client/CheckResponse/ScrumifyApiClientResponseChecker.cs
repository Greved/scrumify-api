using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Scrumify.Api.Client.Exceptions;

namespace Scrumify.Api.Client.CheckResponse
{
    public class ScrumifyApiClientResponseChecker : IScrumifyApiClientResponseChecker
    {
        public async Task EnsureSuccessAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.UnprocessableEntity:
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var validationErrorDetails = JsonConvert.DeserializeObject<ValidationErrorDetails>(errorContent);
                    var errors = validationErrorDetails?.errors?.apiValidations ?? new List<string>(0);
                    throw new ScrumifyApiClientValidationException(errors);
                }
                case HttpStatusCode.BadRequest:
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var badRequestErrorDetails = JsonConvert.DeserializeObject<BadRequestErrorDetails>(errorContent);
                    throw new ScrumifyApiClientException(badRequestErrorDetails?.messages?.FirstOrDefault() ?? "Bad request: something went wrong");
                }
                default:
                    throw new ScrumifyApiClientException("Something went wrong");
            }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private class ValidationErrorDetails
        {
            public ValidationErrors errors { get; set; }

            public class ValidationErrors
            {
                public List<string> apiValidations { get; set; }
            }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private class BadRequestErrorDetails
        {
            public List<string> messages { get; set; }
        }
    }
}
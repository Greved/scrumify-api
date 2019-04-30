using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Scrumify.Api.Infrastructure.ActionResults
{
    public class ValidationErrorObjectResult: ObjectResult
    {
        public ValidationErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
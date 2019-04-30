using System.Linq;
using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Scrumify.Api.Business.Common.Exceptions;
using Scrumify.Api.Infrastructure.ActionResults;

namespace Scrumify.Api.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment env;
        private readonly ILogger<HttpGlobalExceptionFilter> logger;

        public HttpGlobalExceptionFilter(IHostingEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);


            if (context.Exception is ApiException apiException)
            {
                var problemDetails = new JsonErrorResponse
                {
                    Messages = new[] {apiException.Message}
                };
                context.Result = new BadRequestObjectResult(problemDetails);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception is ValidationException validationException)
            {
                var problemDetails = new ValidationProblemDetails()
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Detail = validationException.Message
                };
                problemDetails.Errors.Add("apiValidations", validationException.Errors.Select(x => x.ToString()).ToArray());

                context.Result = new ValidationErrorObjectResult(problemDetails);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            }
            else
            {
                var json = new JsonErrorResponse
                {
                    Messages = new[] { "An error occur.Try it again." }
                };

                if (env.IsDevelopment())
                {
                    json.DeveloperMessage = context.Exception;
                }

                context.Result = new InternalServerErrorObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.ExceptionHandled = true;
        }

        private class JsonErrorResponse
        {
            public string[] Messages { get; set; }

            public object DeveloperMessage { get; set; }
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using Greved.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Scrumify.Api.Business.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> logger;
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => this.logger = logger;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestGenericTypeName = request.GetGenericTypeName();
            logger.LogInformation("----- Handling command {CommandName} ({@Command})", requestGenericTypeName, request);
            var response = await next();
            logger.LogInformation("----- Command {CommandName} handled - response: {@Response}", requestGenericTypeName, response);

            return response;
        }
    }
}
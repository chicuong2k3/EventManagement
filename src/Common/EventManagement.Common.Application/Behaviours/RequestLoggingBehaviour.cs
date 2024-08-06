using EventManagement.Common.Domain.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace EventManagement.Common.Application.Behaviours
{
    internal sealed class RequestLoggingBehaviour<TRequest, TResponse>(ILogger<RequestLoggingBehaviour<TRequest, TResponse>> logger)
         : IPipelineBehavior<TRequest, TResponse>
         where TRequest : class
         where TResponse : Result
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            string moduleName = GetModuleName(typeof(TRequest).FullName!);
            string requestName = typeof(TRequest).Name;

            using (LogContext.PushProperty("Module", moduleName))
            {
                logger.LogInformation("Processing request {RequestName}", requestName);

                var response = await next();

                if (response.IsSuccess)
                {
                    logger.LogInformation("Completed request {RequestName}", requestName);
                }
                else
                {
                    using (LogContext.PushProperty("Error", response.Error, true))
                    {
                        logger.LogError("Request {RequestName} returns error", requestName);
                    }
                }

                return response;
            }

            
        }

        private string GetModuleName(string requestName) => requestName.Split('.')[2];
    }
}

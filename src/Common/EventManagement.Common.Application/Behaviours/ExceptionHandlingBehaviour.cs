using EventManagement.Common.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventManagement.Common.Application.Behaviours
{
    internal sealed class ExceptionHandlingBehaviour<TRequest, TResponse>(ILogger<ExceptionHandlingBehaviour<TRequest, TResponse>> logger)
         : IPipelineBehavior<TRequest, TResponse>
         where TRequest : class
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                logger.LogError(ex, "Unhandled exception for {RequestName}", requestName);
                throw new InternalServerException(requestName, innerException: ex);
            }
        }
    }
}

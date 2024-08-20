using EventManagement.Common.Application.Caching;
using EventManagement.Common.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventManagement.Common.Application.Behaviours
{
    internal sealed class QueryCachingBehaviour<TRequest, TResponse>(
        ILogger<QueryCachingBehaviour<TRequest, TResponse>> logger,
        ICacheService cacheService)
         : IPipelineBehavior<TRequest, TResponse>
         where TRequest : ICachedQuery
         where TResponse : Result
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var cachedResult = await cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);

            if (cachedResult != null)
            {
                logger.LogInformation("Cache hit for {Query}", typeof(TRequest).Name);
                return cachedResult;
            }

            logger.LogInformation("Cache miss for {Query}", typeof(TRequest).Name);

            var result = await next();

            if (result.IsSuccess)
            {
                await cacheService.SetAsync(request.CacheKey, result, request.Expiration, cancellationToken);
            }

            return result;
        }
    }
}

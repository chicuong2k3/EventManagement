using EventManagement.Common.Application.Messaging;

namespace EventManagement.Common.Application.Caching;

public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery
{
}
public interface ICachedQuery
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
}

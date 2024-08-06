using Microsoft.Extensions.Caching.Distributed;

namespace EventManagement.Common.Infrastructure.Caching
{
    public class CacheOptions
    {
        public static DistributedCacheEntryOptions DefaultExpiration => new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        public static DistributedCacheEntryOptions Create(TimeSpan? expiration) =>
        expiration != null ? new()
        {
            AbsoluteExpirationRelativeToNow = expiration
        } : DefaultExpiration;
    }
}

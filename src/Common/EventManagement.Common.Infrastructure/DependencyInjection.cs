using EventManagement.Common.Application.Caching;
using EventManagement.Common.Application.Data;
using EventManagement.Common.Infrastructure.Caching;
using EventManagement.Common.Infrastructure.Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using StackExchange.Redis;

namespace EventManagement.Common.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureCommon(
            this IServiceCollection services,
            string dbConnectionString,
            string cacheConnectionString)
        {

            // Dapper
            var dataSource = new NpgsqlDataSourceBuilder(dbConnectionString).Build();
            services.TryAddSingleton(dataSource);
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

            // Redis caching
            services.TryAddSingleton<ICacheService, CacheService>();
            var connectionMultiplexer = ConnectionMultiplexer.Connect(cacheConnectionString);
            services.TryAddSingleton(connectionMultiplexer);
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConnectionMultiplexerFactory = () 
                => Task.FromResult<IConnectionMultiplexer>(connectionMultiplexer);
            });

            return services;
        }
    }
}

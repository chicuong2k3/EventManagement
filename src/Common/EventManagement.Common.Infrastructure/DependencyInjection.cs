using EventManagement.Common.Application.Caching;
using EventManagement.Common.Application.Data;
using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Infrastructure.Caching;
using EventManagement.Common.Infrastructure.Dapper;
using EventManagement.Common.Infrastructure.EventBuses;
using EventManagement.Common.Infrastructure.Interceptors;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using StackExchange.Redis;

namespace EventManagement.Common.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCommonInfrastructure(
            this IServiceCollection services,
            Action<IRegistrationConfigurator>[] configureConsumers,    
            string dbConnectionString,
            string cacheConnectionString)
        {

            // Dapper
            var dataSource = new NpgsqlDataSourceBuilder(dbConnectionString).Build();
            services.TryAddSingleton(dataSource);
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

            // Redis caching
            services.TryAddSingleton<ICacheService, CacheService>();

            try
            {
                var connectionMultiplexer = ConnectionMultiplexer.Connect(cacheConnectionString);
                services.TryAddSingleton(connectionMultiplexer);
                services.AddStackExchangeRedisCache(options =>
                {
                    options.ConnectionMultiplexerFactory = ()
                    => Task.FromResult<IConnectionMultiplexer>(connectionMultiplexer);
                });
            }
            catch
            {
                services.AddDistributedMemoryCache();
            }

            // Interceptors
            services.TryAddSingleton<PublishDomainEventsInterceptor>();

            // Messaging
            services.TryAddSingleton<IEventBus, EventBus>();
            services.AddMassTransit(configure =>
            {
                configure.UsingInMemory((context, config) =>
                {
                    config.ConfigureEndpoints(context);
                });

                configure.SetKebabCaseEndpointNameFormatter();

                foreach (var configureConsumer in configureConsumers)
                {
                    configureConsumer(configure);
                }

                
            });

            return services;
        }
    }
}

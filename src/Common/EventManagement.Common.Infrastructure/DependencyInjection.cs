using EventManagement.Common.Application.Data;
using EventManagement.Common.Infrastructure.Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace EventManagement.Common.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureCommon(
            this IServiceCollection services,
            string connectionString)
        {

            // Dapper
            var dataSource = new NpgsqlDataSourceBuilder(connectionString).Build();
            services.TryAddSingleton(dataSource);
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

            return services;
        }
    }
}

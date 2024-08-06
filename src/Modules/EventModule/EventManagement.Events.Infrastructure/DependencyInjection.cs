using EventManagement.Common.Infrastructure;
using EventManagement.Common.Infrastructure.Interceptors;
using EventManagement.Events.Application.Abstractions.Data;
using EventManagement.Events.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Events.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string dbConnectionString,
            string cacheConnectionString)
        {

            services.AddInfrastructureCommon(dbConnectionString, cacheConnectionString);

            // Entity Framework Core
            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(dbConnectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events);
                })
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(serviceProvider.GetRequiredService<PublishDomainEventsInterceptor>());
            });

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();


            services.AddScoped<IUnitOfWork>(serviceProvider =>
            {
                return serviceProvider.GetRequiredService<AppDbContext>();
            });


            return services;
        }
    }
}

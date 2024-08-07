using EventManagement.Common.Infrastructure;
using EventManagement.Common.Infrastructure.Interceptors;
using EventManagement.Events.Application.Abstractions.Data;
using EventManagement.Events.Infrastructure.PublicApi;
using EventManagement.Events.Infrastructure.Repositories;
using EventManagement.Events.PublicApi;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Events.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEventsInfrastructure(
            this IServiceCollection services,
            string dbConnectionString)
        {

            // Entity Framework Core
            services.AddDbContext<EventsDbContext>((serviceProvider, options) =>
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
            services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();


            services.AddScoped<IUnitOfWork>(serviceProvider =>
            {
                return serviceProvider.GetRequiredService<EventsDbContext>();
            });

            // Public Api
            services.AddScoped<IEventsApi, EventsApi>();


            return services;
        }
    }
}

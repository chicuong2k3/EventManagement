using EventManagement.Common.Infrastructure;
using EventManagement.Events.Application.Abstractions;
using EventManagement.Events.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
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
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(dbConnectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events);
                })
                .UseSnakeCaseNamingConvention()
                .AddInterceptors();
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

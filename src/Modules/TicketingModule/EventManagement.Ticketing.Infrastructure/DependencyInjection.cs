using EventManagement.Common.Infrastructure.Interceptors;
using EventManagement.Ticketing.Application.Abstractions.Data;
using EventManagement.Ticketing.Application.IntegrationEventComsumers;
using EventManagement.Ticketing.Application.UseCases.Carts;
using EventManagement.Ticketing.Infrastructure.Data;
using EventManagement.Ticketing.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Ticketing.Infrastructure
{
    
    public static class DependencyInjection
    {
        public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
        {
            registrationConfigurator.AddConsumer<UserRegisteredIntegrationEventComsumer>();
        }
        public static IServiceCollection AddTicketingInfrastructure(
            this IServiceCollection services,
            string dbConnectionString)
        {

            // Entity Framework Core
            services.AddDbContext<TicketingDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(dbConnectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Ticketing);
                })
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(serviceProvider.GetRequiredService<PublishDomainEventsInterceptor>());
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();


            services.AddScoped<IUnitOfWork>(serviceProvider =>
            {
                return serviceProvider.GetRequiredService<TicketingDbContext>();
            });

            services.AddSingleton<CartService>();

            
            return services;
        }
    }
}

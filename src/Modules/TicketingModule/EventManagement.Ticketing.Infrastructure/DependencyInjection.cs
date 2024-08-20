using EventManagement.Common.Application.Messaging;
using EventManagement.Common.Infrastructure.Outbox;
using EventManagement.Ticketing.Application.Abstractions.Data;
using EventManagement.Ticketing.Application.IntegrationEventComsumers;
using EventManagement.Ticketing.Application.Services;
using EventManagement.Ticketing.Domain.Customers;
using EventManagement.Ticketing.Domain.Events;
using EventManagement.Ticketing.Domain.Orders;
using EventManagement.Ticketing.Domain.Payments;
using EventManagement.Ticketing.Domain.Tickets;
using EventManagement.Ticketing.Domain.TicketTypes;
using EventManagement.Ticketing.Infrastructure.Customers;
using EventManagement.Ticketing.Infrastructure.Data;
using EventManagement.Ticketing.Infrastructure.Events;
using EventManagement.Ticketing.Infrastructure.Orders;
using EventManagement.Ticketing.Infrastructure.Outbox;
using EventManagement.Ticketing.Infrastructure.Payments;
using EventManagement.Ticketing.Infrastructure.Tickets;
using EventManagement.Ticketing.Infrastructure.TicketTypes;
using MassTransit;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            IConfiguration configuration)
        {
            // Background Jobs
            services.Configure<OutboxOptions>(configuration.GetSection("Ticketing:Outbox"));

            services.ConfigureOptions<ConfigureProcessOutboxJob>();

            // Entity Framework Core
            string dbConnectionString = configuration.GetConnectionString("Database")!;
            services.AddDbContext<TicketingDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(dbConnectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Ticketing);
                })
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(serviceProvider.GetRequiredService<InsertOutboxMessagesInterceptor>());
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            services.AddScoped<IUnitOfWork>(serviceProvider =>
            {
                return serviceProvider.GetRequiredService<TicketingDbContext>();
            });

            services.AddSingleton<CartService>();
            services.AddSingleton<IPaymentService, PaymentService>();


            services.AddDomainEventHanlers();

            
            return services;
        }

        private static void AddDomainEventHanlers(this IServiceCollection services)
        {
            var domainEventHandlers = Application.AssemblyReference.Assembly.GetTypes()
                .Where(type => type.IsAssignableTo(typeof(IDomainEventHandler)))
                .ToArray();

            foreach (var domainEventHandler in domainEventHandlers)
            {
                services.TryAddScoped(domainEventHandler);

                var domainEvent = domainEventHandler
                    .GetInterfaces()
                    .Single(x => x.IsGenericType)
                    .GetGenericArguments()
                    .Single();

                var closedIdempotentDomainEventHandler = typeof(IdempotentDomainEventHandler<>)
                    .MakeGenericType(domainEvent);

                services.Decorate(domainEventHandler, closedIdempotentDomainEventHandler);
            }


        }
    }
}

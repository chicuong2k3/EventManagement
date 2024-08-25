using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Messaging;
using EventManagement.Common.Infrastructure.Outbox;
using EventManagement.Events.Application.Abstractions.Data;
using EventManagement.Events.Application.Events.CancelEvent.Saga;
using EventManagement.Events.Domain.Categories;
using EventManagement.Events.Domain.Events;
using EventManagement.Events.Domain.TicketTypes;
using EventManagement.Events.Infrastructure.Categories;
using EventManagement.Events.Infrastructure.Events;
using EventManagement.Events.Infrastructure.Inbox;
using EventManagement.Events.Infrastructure.Outbox;
using EventManagement.Events.Infrastructure.TicketTypes;
using MassTransit;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventManagement.Events.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEventsInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Background Jobs
            services.Configure<OutboxOptions>(configuration.GetSection("Events:Outbox"));

            services.ConfigureOptions<ConfigureProcessOutboxJob>();

            services.Configure<InboxOptions>(configuration.GetSection("Events:Inbox"));

            services.ConfigureOptions<ConfigureProcessInboxJob>();

            // Entity Framework Core
            string dbConnectionString = configuration.GetConnectionString("Database")!;
            services.AddDbContext<EventsDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(dbConnectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events);
                })
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(serviceProvider.GetRequiredService<InsertOutboxMessagesInterceptor>());
            });

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();


            services.AddScoped<IUnitOfWork>(serviceProvider =>
            {
                return serviceProvider.GetRequiredService<EventsDbContext>();
            });


            services.AddDomainEventHanlers();

            services.AddIntegrationEventHandlers();

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

        public static Action<IRegistrationConfigurator> ConfigureMassTransitHandlers(string redisConnectionString)
        {
            return registrationConfigurator => 
                registrationConfigurator.AddSagaStateMachine<CancelEventSaga, CancelEventState>()
                    .RedisRepository(redisConnectionString);
        }

        private static void AddIntegrationEventHandlers(this IServiceCollection services)
        {
            var integrationEventHandlers = Application.AssemblyReference.Assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler)))
                .ToArray();

            foreach (var integrationEventHandler in integrationEventHandlers)
            {
                services.TryAddScoped(integrationEventHandler);

                var integrationEvent = integrationEventHandler
                    .GetInterfaces()
                    .Single(i => i.IsGenericType)
                    .GetGenericArguments()
                    .Single();

                var closedIdempotentHandler =
                    typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEvent);

                services.Decorate(integrationEventHandler, closedIdempotentHandler);
            }
        }
    }
}

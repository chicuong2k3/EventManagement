using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Messaging;
using EventManagement.Common.Infrastructure.Outbox;
using EventManagement.Users.Application.Abstractions.Data;
using EventManagement.Users.Application.Abstractions.Identity;
using EventManagement.Users.Domain.Users;
using EventManagement.Users.Infrastructure.Authorization;
using EventManagement.Users.Infrastructure.Identity;
using EventManagement.Users.Infrastructure.Inbox;
using EventManagement.Users.Infrastructure.Outbox;
using EventManagement.Users.Infrastructure.Users;
using EventManagement.Users.IntegrationEvents;
using MassTransit;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace EventManagement.Users.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUsersInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Background Jobs
            services.Configure<OutboxOptions>(configuration.GetSection("Users:Outbox"));
            
            services.ConfigureOptions<ConfigureProcessOutboxJob>();

            // KeyCloak
            services.Configure<KeyCloakOptions>(configuration.GetSection("Users:KeyCloak"));

            services.AddTransient<KeyCloakAuthDelegatingHandler>();

            services.AddHttpClient<KeyCloakClient>((serviceProvider, httpClient) =>
            {
                var keyCloakOptions = serviceProvider
                    .GetRequiredService<IOptions<KeyCloakOptions>>().Value;

                httpClient.BaseAddress = new Uri(keyCloakOptions.AdminUrl);
            })
            .AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>();

            services.AddTransient<IIdentityProviderService, IdentityProviderService>();

            // Authorization
            services.AddScoped<IPermissionService, PermissionService>();

            // Entity Framework Core
            string dbConnectionString = configuration.GetConnectionString("Database")!;
            services.AddDbContext<UsersDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(dbConnectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Users);
                })
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(serviceProvider.GetRequiredService<InsertOutboxMessagesInterceptor>());
            });

            services.AddScoped<IUserRepository, UserRepository>();


            services.AddScoped<IUnitOfWork>(serviceProvider =>
            {
                return serviceProvider.GetRequiredService<UsersDbContext>();
            });


            services.AddDomainEventHanlers();

            services.AddIntegrationEventConsumers();

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

        public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
        {
        }

        private static void AddIntegrationEventConsumers(this IServiceCollection services)
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

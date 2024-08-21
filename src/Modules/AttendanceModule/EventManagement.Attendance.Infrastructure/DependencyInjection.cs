using EventManagement.Attendance.Application.Abstractions.Data;
using EventManagement.Attendance.Infrastructure.Attendees;
using EventManagement.Attendance.Infrastructure.Authentication;
using EventManagement.Attendance.Infrastructure.Data;
using EventManagement.Attendance.Infrastructure.Events;
using EventManagement.Attendance.Infrastructure.Inbox;
using EventManagement.Attendance.Infrastructure.Outbox;
using EventManagement.Attendance.Infrastructure.Tickets;
using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Messaging;
using EventManagement.Common.Infrastructure.Outbox;
using MassTransit;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventManagement.Attendance.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAttendanceInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Background Jobs
        services.Configure<OutboxOptions>(configuration.GetSection("Attendance:Outbox"));

        services.ConfigureOptions<ConfigureProcessOutboxJob>();

        services.Configure<InboxOptions>(configuration.GetSection("Attendance:Inbox"));

        services.ConfigureOptions<ConfigureProcessInboxJob>();
        // Database
        services.AddDbContext<AttendanceDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    configuration.GetConnectionString("Database"),
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Attendance))
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>()));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AttendanceDbContext>());

        services.AddScoped<IAttendeeRepository, AttendeeRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

        services.AddScoped<IAttendanceContext, AttendanceContext>();


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

    public static void ConfigureIntegrationEventHandlers(IRegistrationConfigurator registrationConfigurator)
    {
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

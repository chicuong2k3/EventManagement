using EventManagement.Common.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Common.Infrastructure.Interceptors;

public sealed class PublishDomainEventsInterceptor(IServiceScopeFactory serviceScopeFactory)
    : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context != null)
        {
            await PublishDomainEventsAsync(eventData.Context);
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEventsAsync(DbContext context)
    {
        var domainEvents = context.ChangeTracker
            .Entries<Entity>()
            .Select(x => x.Entity)
            .SelectMany(x =>
            {
                var domainEvents = x.DomainEvents.ToList();
                x.ClearDomainEvents();
                return domainEvents;
            })
            .ToList();

        using (var scope = serviceScopeFactory.CreateScope())
        {
            var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

            foreach (var domainEvent in domainEvents)
            {
                await publisher.Publish(domainEvent);
            }
        }

    }
}

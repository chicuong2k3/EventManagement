using EventManagement.Common.Application.EventBuses;
using EventManagement.Ticketing.Domain.Events;
using EventManagement.Ticketing.IntegrationEvents;

namespace EventManagement.Ticketing.Application.Tickets.ArchiveTicketsForEvent;

internal sealed class EventTicketsArchivedDomainEventHandler(IEventBus eventBus)
     : DomainEventHandler<EventTicketsArchivedDomainEvent>
{
    public override async Task Handle(
        EventTicketsArchivedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new EventTicketsArchivedIntegrationEvent(
                domainEvent.EventId,
                domainEvent.OccurredOn,
                domainEvent.EventId),
            cancellationToken);
    }
}

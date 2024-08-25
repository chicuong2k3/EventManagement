using EventManagement.Common.Application.EventBuses;
using EventManagement.Ticketing.Domain.Tickets;
using EventManagement.Ticketing.IntegrationEvents;

namespace EventManagement.Ticketing.Application.Tickets.ArchiveTicket;

internal sealed class TicketArchivedDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<TicketArchivedDomainEvent>
{
    public override async Task Handle(
        TicketArchivedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new TicketArchivedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOn,
                domainEvent.TicketId,
                domainEvent.Code),
            cancellationToken);
    }
}

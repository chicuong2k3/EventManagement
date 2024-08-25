using EventManagement.Common.Application.EventBuses;
using EventManagement.Ticketing.Domain.TicketTypes;
using EventManagement.Ticketing.IntegrationEvents;

namespace EventManagement.Ticketing.Application.TicketTypes;

internal sealed class TicketTypeSoldOutDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<TicketTypeSoldOutDomainEvent>
{
    public override async Task Handle(
        TicketTypeSoldOutDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new TicketTypeSoldOutIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOn,
                domainEvent.TicketTypeId),
            cancellationToken);
    }
}

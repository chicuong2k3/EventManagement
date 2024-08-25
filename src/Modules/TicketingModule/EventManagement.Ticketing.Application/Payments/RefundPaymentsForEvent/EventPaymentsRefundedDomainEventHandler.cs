using EventManagement.Common.Application.EventBuses;
using EventManagement.Ticketing.Domain.Events;
using EventManagement.Ticketing.IntegrationEvents;

namespace EventManagement.Ticketing.Application.Payments.RefundPaymentsForEvent;

internal sealed class EventPaymentsRefundedDomainEventHandler(IEventBus eventBus)
     : DomainEventHandler<EventPaymentsRefundedDomainEvent>
{
    public override async Task Handle(
        EventPaymentsRefundedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new EventPaymentsRefundedIntegrationEvent(
                domainEvent.EventId,
                domainEvent.OccurredOn,
                domainEvent.EventId),
            cancellationToken);
    }
}

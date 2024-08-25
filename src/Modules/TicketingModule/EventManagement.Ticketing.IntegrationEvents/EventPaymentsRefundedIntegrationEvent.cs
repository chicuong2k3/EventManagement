using EventManagement.Common.Application.EventBuses;

namespace EventManagement.Ticketing.IntegrationEvents;

public sealed class EventPaymentsRefundedIntegrationEvent : IntegrationEvent
{
    public EventPaymentsRefundedIntegrationEvent(Guid id, DateTime occurredOn, Guid eventId)
        : base(id, occurredOn)
    {
        EventId = eventId;
    }

    public Guid EventId { get; init; }
}

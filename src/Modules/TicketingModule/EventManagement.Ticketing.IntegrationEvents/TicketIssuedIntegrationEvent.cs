using EventManagement.Common.Application.EventBuses;

namespace EventManagement.Ticketing.IntegrationEvents;

public sealed class TicketIssuedIntegrationEvent : IntegrationEvent
{
    public TicketIssuedIntegrationEvent(
        Guid id,
        DateTime occurredOn,
        Guid ticketId,
        Guid customerId,
        Guid eventId,
        string code)
        : base(id, occurredOn)
    {
        TicketId = ticketId;
        CustomerId = customerId;
        EventId = eventId;
        Code = code;
    }

    public Guid TicketId { get; init; }

    public Guid CustomerId { get; init; }

    public Guid EventId { get; init; }

    public string Code { get; init; }
}

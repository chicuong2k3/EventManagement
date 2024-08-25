using EventManagement.Common.Application.EventBuses;

namespace EventManagement.Ticketing.IntegrationEvents;

public sealed class TicketArchivedIntegrationEvent : IntegrationEvent
{
    public TicketArchivedIntegrationEvent(
        Guid id,
        DateTime occurredOn,
        Guid ticketId,
        string code)
        : base(id, occurredOn)
    {
        TicketId = ticketId;
        Code = code;
    }

    public Guid TicketId { get; init; }

    public string Code { get; init; }
}

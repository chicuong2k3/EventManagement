using EventManagement.Common.Domain.DomainEvents;

namespace EventManagement.Attendance.Domain.Tickets;

public sealed class TicketCreatedDomainEvent(Guid ticketId, Guid eventId) : DomainEventBase
{
    public Guid TicketId { get; init; } = ticketId;

    public Guid EventId { get; init; } = eventId;
}

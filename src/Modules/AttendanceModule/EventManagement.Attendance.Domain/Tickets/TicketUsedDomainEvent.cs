using EventManagement.Common.Domain.DomainEvents;

namespace EventManagement.Attendance.Domain.Tickets;

public sealed class TicketUsedDomainEvent(Guid ticketId) : DomainEventBase
{
    public Guid TicketId { get; init; } = ticketId;
}

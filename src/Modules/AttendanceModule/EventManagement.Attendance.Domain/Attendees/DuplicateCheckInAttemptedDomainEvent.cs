using EventManagement.Common.Domain.DomainEvents;

namespace EventManagement.Attendance.Domain.Attendees;

public sealed class DuplicateCheckInAttemptedDomainEvent(Guid attendeeId, Guid eventId, Guid ticketId, string ticketCode)
    : DomainEventBase
{
    public Guid AttendeeId { get; init; } = attendeeId;

    public Guid EventId { get; init; } = eventId;

    public Guid TicketId { get; init; } = ticketId;

    public string TicketCode { get; init; } = ticketCode;
}

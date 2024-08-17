using EventManagement.Common.Domain.DomainEvents;

namespace EventManagement.Attendance.Domain.Attendees;

public sealed class AttendeeCheckedInDomainEvent(Guid attendeeId, Guid eventId) : DomainEventBase
{
    public Guid AttendeeId { get; init; } = attendeeId;

    public Guid EventId { get; init; } = eventId;
}

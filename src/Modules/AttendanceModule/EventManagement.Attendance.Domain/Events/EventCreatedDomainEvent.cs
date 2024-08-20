using EventManagement.Common.Domain;

namespace EventManagement.Attendance.Domain.Events;

public sealed class EventCreatedDomainEvent(
    Guid eventId,
    string title,
    string description,
    string location,
    DateTime startsAt,
    DateTime? endsAt) : DomainEvent
{
    public Guid EventId { get; init; } = eventId;

    public string Title { get; init; } = title;

    public string Description { get; init; } = description;

    public string Location { get; init; } = location;

    public DateTime StartsAt { get; init; } = startsAt;

    public DateTime? EndsAt { get; init; } = endsAt;
}

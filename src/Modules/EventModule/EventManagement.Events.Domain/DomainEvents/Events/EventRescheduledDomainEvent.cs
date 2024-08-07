namespace EventManagement.Events.Domain.DomainEvents.Events;

public sealed class EventRescheduledDomainEvent(
    Guid eventId,
    DateTime startsAt,
    DateTime? endsAt) : DomainEventBase
{
    public Guid EventId { get; } = eventId;

    public DateTime StartsAt { get; } = startsAt;

    public DateTime? EndsAt { get; } = endsAt;
}

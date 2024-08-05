namespace EventManagement.Events.Domain.DomainEvents.Events;

public sealed class EventCancelled(Guid eventId) : DomainEventBase
{
    public Guid EventId { get; init; } = eventId;
}

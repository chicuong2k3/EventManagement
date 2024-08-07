

namespace EventManagement.Events.Domain.DomainEvents.Events;
public sealed class EventPublishedDomainEvent(Guid eventId) : DomainEventBase
{
    public Guid EventId { get; init; } = eventId;
}

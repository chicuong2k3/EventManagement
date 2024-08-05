

namespace EventManagement.Events.Domain.DomainEvents.Events;
public sealed class EventPublished(Guid eventId) : DomainEventBase
{
    public Guid EventId { get; init; } = eventId;
}

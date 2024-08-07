namespace EventManagement.Events.Domain.DomainEvents.Events
{
    public sealed class EventCancelledDomainEvent(Guid eventId) : DomainEventBase
    {
        public Guid EventId { get; init; } = eventId;
    }
}

namespace EventManagement.Events.Domain.DomainEvents.Events
{
    public sealed class EventCreatedDomainEvent(Guid eventId) : DomainEventBase
    {
        public Guid EventEntityId { get; init; } = eventId;
    }
}

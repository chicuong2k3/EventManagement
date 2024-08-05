namespace EventManagement.Events.Domain.DomainEvents.Events
{
    public sealed class EventCreated(Guid eventId) : DomainEventBase
    {
        public Guid EventEntityId { get; init; } = eventId;
    }
}

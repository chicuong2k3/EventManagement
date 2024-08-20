namespace EventManagement.Events.Domain.Events
{
    public sealed class EventCreatedDomainEvent(Guid eventId) : DomainEvent
    {
        public Guid EventEntityId { get; init; } = eventId;
    }
}

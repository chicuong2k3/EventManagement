namespace EventManagement.Ticketing.Domain.DomainEvents.Events
{
    public sealed class EventCancelledDomainEvent(Guid eventId) : DomainEventBase
    {
        public Guid EventId { get; } = eventId;
    }
}

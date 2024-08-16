namespace EventManagement.Ticketing.Domain.DomainEvents.Events
{
    public sealed class EventTicketsArchivedDomainEvent(Guid eventId) : DomainEventBase
    {
        public Guid EventId { get; } = eventId;
    }
}

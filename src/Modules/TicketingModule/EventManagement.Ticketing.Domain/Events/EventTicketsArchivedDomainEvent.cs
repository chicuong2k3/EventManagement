namespace EventManagement.Ticketing.Domain.Events
{
    public sealed class EventTicketsArchivedDomainEvent(Guid eventId) : DomainEvent
    {
        public Guid EventId { get; } = eventId;
    }
}

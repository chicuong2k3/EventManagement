namespace EventManagement.Ticketing.Domain.Events
{
    public sealed class EventPaymentsRefundedDomainEvent(Guid eventId) : DomainEvent
    {
        public Guid EventId { get; } = eventId;
    }
}

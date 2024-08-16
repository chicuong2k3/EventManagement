namespace EventManagement.Ticketing.Domain.DomainEvents.Events
{
    public sealed class EventPaymentsRefundedDomainEvent(Guid eventId) : DomainEventBase
    {
        public Guid EventId { get; } = eventId;
    }
}

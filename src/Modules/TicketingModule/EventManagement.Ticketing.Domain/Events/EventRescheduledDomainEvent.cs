namespace EventManagement.Ticketing.Domain.Events
{
    public sealed class EventRescheduledDomainEvent(
        Guid eventId,
        DateTime startsAt,
        DateTime? endsAt) : DomainEvent
    {
        public Guid EventId { get; } = eventId;
        public DateTime StartsAt { get; } = startsAt;
        public DateTime? EndsAt { get; } = endsAt;
    }
}

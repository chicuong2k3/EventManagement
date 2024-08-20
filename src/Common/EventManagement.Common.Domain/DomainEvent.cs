namespace EventManagement.Common.Domain
{
    public abstract class DomainEvent : IDomainEvent
    {
        public Guid Id { get; init; }

        public DateTime OccurredOn { get; init; }
        protected DomainEvent()
        {
            Id = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
        }
        protected DomainEvent(Guid id, DateTime occurredOn)
        {
            Id = id;
            OccurredOn = occurredOn;
        }
    }
}

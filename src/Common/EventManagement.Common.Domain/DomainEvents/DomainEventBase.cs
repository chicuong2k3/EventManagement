namespace EventManagement.Common.Domain.DomainEvents
{
    public abstract class DomainEventBase : IDomainEvent
    {
        public Guid Id { get; init; }

        public DateTime OccurredOn { get; init; }
        protected DomainEventBase()
        {
            Id = Guid.NewGuid();
            OccurredOn = DateTime.Now;
        }
        protected DomainEventBase(Guid id, DateTime occurredOn)
        {
            Id = id;
            OccurredOn = occurredOn;
        }
    }
}

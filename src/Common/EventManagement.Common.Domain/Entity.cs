namespace EventManagement.Common.Domain
{
    public abstract class Entity
    {
        private readonly List<IDomainEvent> domainEvents = [];
        public IReadOnlyCollection<IDomainEvent> DomainEvents => [.. domainEvents];
        protected Entity()
        {

        }

        public void ClearDomainEvents()
        {
            domainEvents.Clear();
        }

        protected void Raise(IDomainEvent domainEvent)
        {
            domainEvents.Add(domainEvent);
        }
    }
}

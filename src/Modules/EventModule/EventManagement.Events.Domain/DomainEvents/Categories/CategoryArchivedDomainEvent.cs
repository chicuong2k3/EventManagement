namespace EventManagement.Events.Domain.DomainEvents.Categories
{
    public sealed class CategoryArchivedDomainEvent(Guid categoryId) : DomainEventBase
    {
        public Guid CategoryId { get; init; } = categoryId;
    }
}

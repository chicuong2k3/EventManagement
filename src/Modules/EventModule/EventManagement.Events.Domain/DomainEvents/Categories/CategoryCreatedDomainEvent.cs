namespace EventManagement.Events.Domain.DomainEvents.Categories
{
    public sealed class CategoryCreatedDomainEvent(Guid categoryId) : DomainEventBase
    {
        public Guid CategoryId { get; init; } = categoryId;
    }
}

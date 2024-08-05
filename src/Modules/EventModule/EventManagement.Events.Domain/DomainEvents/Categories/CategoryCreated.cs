namespace EventManagement.Events.Domain.DomainEvents.Categories
{
    public sealed class CategoryCreated(Guid categoryId) : DomainEventBase
    {
        public Guid CategoryId { get; init; } = categoryId;
    }
}

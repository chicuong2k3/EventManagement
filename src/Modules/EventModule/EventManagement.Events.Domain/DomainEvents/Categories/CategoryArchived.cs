namespace EventManagement.Events.Domain.DomainEvents.Categories
{
    public sealed class CategoryArchived(Guid categoryId) : DomainEventBase
    {
        public Guid CategoryId { get; init; } = categoryId;
    }
}

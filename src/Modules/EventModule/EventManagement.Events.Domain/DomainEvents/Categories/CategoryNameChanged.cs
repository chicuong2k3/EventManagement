namespace EventManagement.Events.Domain.DomainEvents.Categories
{
    public sealed class CategoryNameChanged(Guid categoryId, string categoryName) 
        : DomainEventBase
    {
        public Guid CategoryId { get; init; } = categoryId;
        public string CategoryName { get; init; } = categoryName;
    }
}

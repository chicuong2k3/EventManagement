namespace EventManagement.Events.Domain.Categories
{
    public sealed class CategoryNameChangedDomainEvent(Guid categoryId, string categoryName)
        : DomainEvent
    {
        public Guid CategoryId { get; init; } = categoryId;
        public string CategoryName { get; init; } = categoryName;
    }
}

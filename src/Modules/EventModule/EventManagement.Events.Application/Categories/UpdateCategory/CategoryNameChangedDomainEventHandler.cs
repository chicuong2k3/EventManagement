using EventManagement.Events.Domain.Categories;

namespace EventManagement.Events.Application.Categories.UpdateCategory
{
    internal class CategoryNameChangedDomainEventHandler(
        IEventBus eventBus,
        ISender sender) : DomainEventHandler<CategoryNameChangedDomainEvent>
    {
        public override Task Handle(CategoryNameChangedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

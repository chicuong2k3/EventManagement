using EventManagement.Events.Domain.Categories;

namespace EventManagement.Events.Application.Categories.CreateCategory
{
    internal class CategoryCreatedDomainEventHandler(
        IEventBus eventBus,
        ISender sender) : DomainEventHandler<CategoryCreatedDomainEvent>
    {
        public override Task Handle(CategoryCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

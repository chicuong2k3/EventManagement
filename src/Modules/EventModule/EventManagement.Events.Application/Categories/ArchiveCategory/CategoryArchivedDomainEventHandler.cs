using EventManagement.Events.Domain.Categories;

namespace EventManagement.Events.Application.Categories.ArchiveCategory
{
    internal class CategoryArchivedDomainEventHandler(
        IEventBus eventBus,
        ISender sender) : DomainEventHandler<CategoryArchivedDomainEvent>
    {
        public override Task Handle(CategoryArchivedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

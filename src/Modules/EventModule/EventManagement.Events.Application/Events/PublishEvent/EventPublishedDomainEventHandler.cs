namespace EventManagement.Events.Application.Events.PublishEvent
{
    internal class EventPublishedDomainEventHandler(
        IEventBus eventBus,
        ISender sender) : DomainEventHandler<EventPublishedDomainEvent>
    {
        public override Task Handle(EventPublishedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

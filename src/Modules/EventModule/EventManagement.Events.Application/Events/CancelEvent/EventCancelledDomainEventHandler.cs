namespace EventManagement.Events.Application.Events.CancelEvent
{
    internal class EventCancelledDomainEventHandler(
        IEventBus eventBus,
        ISender sender) : DomainEventHandler<EventCancelledDomainEvent>
    {
        public override Task Handle(EventCancelledDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

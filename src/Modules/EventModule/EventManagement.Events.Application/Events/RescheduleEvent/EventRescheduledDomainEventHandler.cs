

namespace EventManagement.Events.Application.Events.RescheduleEvent
{
    internal class EventRescheduledDomainEventHandler(
        IEventBus eventBus) 
        : DomainEventHandler<EventRescheduledDomainEvent>
    {
        public override async Task Handle(EventRescheduledDomainEvent domainEvent, CancellationToken cancellationToken)
        {
           
            await eventBus.PublishAsync(new EventRescheduledIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOn,
                domainEvent.EventId,
                domainEvent.StartsAt,
                domainEvent.EndsAt
            ));
        }
    }
}

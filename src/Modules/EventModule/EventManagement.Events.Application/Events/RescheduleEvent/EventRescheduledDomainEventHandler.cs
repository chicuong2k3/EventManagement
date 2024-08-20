using EventManagement.Events.Domain.Events;

namespace EventManagement.Events.Application.Events.RescheduleEvent
{
    internal class EventRescheduledDomainEventHandler : DomainEventHandler<EventRescheduledDomainEvent>
    {
        public override Task Handle(EventRescheduledDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

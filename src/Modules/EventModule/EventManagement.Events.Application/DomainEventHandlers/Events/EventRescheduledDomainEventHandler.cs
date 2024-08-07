using EventManagement.Events.Domain.DomainEvents.Events;

namespace EventManagement.Events.Application.DomainEventHandlers.Events
{
    internal class EventRescheduledDomainEventHandler : IDomainEventHandler<EventRescheduledDomainEvent>
    {
        public Task Handle(EventRescheduledDomainEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

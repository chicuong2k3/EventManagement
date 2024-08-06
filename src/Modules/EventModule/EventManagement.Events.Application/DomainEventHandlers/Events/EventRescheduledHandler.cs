using EventManagement.Events.Domain.DomainEvents.Events;

namespace EventManagement.Events.Application.DomainEventHandlers.Events
{
    internal class EventRescheduledHandler : IDomainEventHandler<EventRescheduled>
    {
        public Task Handle(EventRescheduled notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

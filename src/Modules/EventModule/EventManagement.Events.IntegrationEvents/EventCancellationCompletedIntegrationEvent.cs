using EventManagement.Common.Application.EventBuses;

namespace EventManagement.Events.IntegrationEvents
{
    public sealed class EventCancellationCompletedIntegrationEvent
        : IntegrationEvent
    {
        public EventCancellationCompletedIntegrationEvent()
        {
            
        }
        public EventCancellationCompletedIntegrationEvent(Guid id, DateTime occurredOn, Guid eventId)
            : base(id, occurredOn)
        {
            EventId = eventId;
        }
        public Guid EventId { get; init; }
    }
}

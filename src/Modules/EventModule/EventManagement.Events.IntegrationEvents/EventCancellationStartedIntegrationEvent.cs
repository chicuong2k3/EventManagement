using EventManagement.Common.Application.EventBuses;

namespace EventManagement.Events.IntegrationEvents
{
    public sealed class EventCancellationStartedIntegrationEvent
        : IntegrationEvent
    {
        public EventCancellationStartedIntegrationEvent()
        {
            
        }
        public EventCancellationStartedIntegrationEvent(Guid id, DateTime occurredOn, Guid eventId)
            : base(id, occurredOn)
        {
            EventId = eventId;
        }
        public Guid EventId { get; init; }
    }
}

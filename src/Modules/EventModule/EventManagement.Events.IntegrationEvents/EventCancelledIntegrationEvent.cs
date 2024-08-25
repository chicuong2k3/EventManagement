
using EventManagement.Common.Application.EventBuses;

namespace EventManagement.Events.IntegrationEvents
{
    public sealed class EventCancelledIntegrationEvent : IntegrationEvent
    {
        public EventCancelledIntegrationEvent()
        {
            
        }
        public EventCancelledIntegrationEvent(Guid id, DateTime occurredOn, Guid eventId)
            : base(id, occurredOn)
        {
            EventId = eventId;
        }

        public Guid EventId { get; init; }
    }

}

using EventManagement.Common.Application.EventBuses;

namespace EventManagement.Events.IntegrationEvents
{
    public sealed class EventPublishedIntegrationEvent : IntegrationEvent
    {
        public EventPublishedIntegrationEvent()
        {
            
        }

        public EventPublishedIntegrationEvent(
            Guid intergrationEventId,
            DateTime occurredOn,
            Guid eventId,
            string title,
            string description,
            string location,
            DateTime startsAt,
            DateTime? endsAt,
            List<TicketTypeModel> ticketTypes
            ) : base(intergrationEventId, occurredOn)
        {
            EventId = eventId;
            Title = title;
            Description = description;
            Location = location;
            StartsAt = startsAt;
            EndsAt = endsAt;
            TicketTypes = ticketTypes;
        }

        public Guid EventId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Location { get; private set; }
        public DateTime StartsAt { get; private set; }
        public DateTime? EndsAt { get; private set; }
        public List<TicketTypeModel> TicketTypes { get; init; }
    }
}

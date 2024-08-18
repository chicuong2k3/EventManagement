using EventManagement.Common.Application.EventBuses;

namespace EventManagement.Events.IntegrationEvents
{
    public sealed class TicketTypeCreatedIntegrationEvent : IntegrationEvent
    {
        public TicketTypeCreatedIntegrationEvent()
        {
            
        }

        public TicketTypeCreatedIntegrationEvent(
            Guid intergrationEventId,
            DateTime occurredOn,
            Guid ticketTypeId,
            Guid eventId,
            string name,
            decimal price,
            string currency,
            int quantity
            ) : base(intergrationEventId, occurredOn)
        {
            TicketTypeId = ticketTypeId;
            EventId = eventId;
            Name = name;
            Price = price;
            Currency = currency;
            Quantity = quantity;
        }
        public Guid TicketTypeId { get; private set; }

        public Guid EventId { get; private set; }

        public string Name { get; private set; }

        public decimal Price { get; private set; }

        public string Currency { get; private set; }

        public int Quantity { get; private set; }
    }
}

using EventManagement.Common.Application.EventBuses;

namespace EventManagement.Events.IntegrationEvents
{
    public sealed class TicketTypePriceChangedIntegrationEvent : IntegrationEvent
    {
        public TicketTypePriceChangedIntegrationEvent()
        {
            
        }

        public TicketTypePriceChangedIntegrationEvent(
            Guid intergrationEventId,
            DateTime occurredOn,
            Guid ticketTypeId,
            decimal price
            ) : base(intergrationEventId, occurredOn)
        {
            TicketTypeId = ticketTypeId;
            Price = price;
        }
        public Guid TicketTypeId { get; private set; }

        public decimal Price { get; private set; }

    }
}

using EventManagement.Events.Domain.TicketTypes;

namespace EventManagement.Events.Application.TicketTypes.UpdateTicketTypePrice
{
    internal class TicketTypePriceChangedDomainEventHandler(
        IEventBus eventBus) : DomainEventHandler<TicketTypePriceChangedDomainEvent>
    {
        public override async Task Handle(TicketTypePriceChangedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            await eventBus.PublishAsync(new TicketTypePriceChangedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOn,
                domainEvent.TicketTypeId, 
                domainEvent.Price
            ), cancellationToken);
        }
    }
}

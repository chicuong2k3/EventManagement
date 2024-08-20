using EventManagement.Events.Domain.TicketTypes;

namespace EventManagement.Events.Application.TicketTypes.UpdateTicketTypePrice
{
    internal class TicketTypePriceChangedDomainEventHandler(
        IEventBus eventBus,
        ISender sender) : DomainEventHandler<TicketTypePriceChangedDomainEvent>
    {
        public override Task Handle(TicketTypePriceChangedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

using EventManagement.Events.Application.TicketTypes;
using EventManagement.Events.Domain.TicketTypes;

namespace EventManagement.Events.Application.TicketTypes.CreateTicketType
{
    internal class TicketTypeCreatedDomainEventHandler(
        IEventBus eventBus,
        ISender sender)
        : DomainEventHandler<TicketTypeCreatedDomainEvent>
    {
        public override async Task Handle(TicketTypeCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetTicketTypeByIdQuery(domainEvent.TicketTypeId), cancellationToken);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(GetTicketTypeByIdQuery), result.Error);
            }

            await eventBus.PublishAsync(
                new TicketTypeCreatedIntegrationEvent(
                    domainEvent.Id,
                    domainEvent.OccurredOn,
                    result.Value.Id,
                    result.Value.EventId,
                    result.Value.Name,
                    result.Value.Price,
                    result.Value.Currency,
                    result.Value.Quantity
                ), cancellationToken);
        }
    }
}

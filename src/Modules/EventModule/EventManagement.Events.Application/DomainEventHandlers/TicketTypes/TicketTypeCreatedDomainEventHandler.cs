using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Exceptions;
using EventManagement.Events.Application.UseCases.TicketTypes;
using EventManagement.Events.Domain.DomainEvents.TicketTypes;
using EventManagement.Events.IntegrationEvents;
using MediatR;

namespace EventManagement.Events.Application.DomainEventHandlers.TicketTypes
{
    internal class TicketTypeCreatedDomainEventHandler(
        IEventBus eventBus,
        ISender sender)
        : IDomainEventHandler<TicketTypeCreatedDomainEvent>
    {
        public async Task Handle(TicketTypeCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetTicketTypeByIdQuery(notification.TicketTypeId), cancellationToken);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(GetTicketTypeByIdQuery), result.Error);
            }

            await eventBus.PublishAsync(
                new TicketTypeCreatedIntegrationEvent(
                    notification.Id,
                    notification.OccurredOn,
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

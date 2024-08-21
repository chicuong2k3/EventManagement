using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Exceptions;
using EventManagement.Events.IntegrationEvents;
using EventManagement.Ticketing.Application.TicketTypes;
using MediatR;

namespace EventManagement.Ticketing.Application.IntegrationEventComsumers
{
    internal class TicketTypeCreatedIntegrationEventComsumer(ISender sender)
                : IntegrationEventHandler<TicketTypeCreatedIntegrationEvent>
    {
        public override async Task Handle(
            TicketTypeCreatedIntegrationEvent integrationEvent,
            CancellationToken cancellationToken = default)
        {
            var command = new CreateTicketTypeCommand(
                integrationEvent.Id,
                integrationEvent.EventId,
                integrationEvent.Name,
                integrationEvent.Price,
                integrationEvent.Currency,
                integrationEvent.Quantity);

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(CreateTicketTypeCommand), result.Error);
            }
        }
    }
}

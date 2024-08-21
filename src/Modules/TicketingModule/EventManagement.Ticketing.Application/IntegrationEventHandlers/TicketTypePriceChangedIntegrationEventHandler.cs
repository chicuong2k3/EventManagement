using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Exceptions;
using EventManagement.Events.IntegrationEvents;
using EventManagement.Ticketing.Application.TicketTypes;
using MediatR;

namespace EventManagement.Ticketing.Application.IntegrationEventHandlers
{
    internal class TicketTypePriceChangedIntegrationEventHandler(ISender sender)
                : IntegrationEventHandler<TicketTypePriceChangedIntegrationEvent>
    {
        public override async Task Handle(
            TicketTypePriceChangedIntegrationEvent integrationEvent,
            CancellationToken cancellationToken = default)
        {
            var command = new UpdateTicketTypePriceCommand(
                integrationEvent.Id,
                integrationEvent.Price);

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(UpdateTicketTypePriceCommand), result.Error);
            }
        }
    }
}

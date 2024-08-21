using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Exceptions;
using EventManagement.Events.IntegrationEvents;
using EventManagement.Ticketing.Application.Events;
using MediatR;

namespace EventManagement.Ticketing.Application.IntegrationEventHandlers
{
    internal class EventPublishedIntegrationEventHandler(ISender sender)
                : IntegrationEventHandler<EventPublishedIntegrationEvent>
    {
        public override async Task Handle(EventPublishedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var command = new CreateEventCommand(
                integrationEvent.Id,
                integrationEvent.Title,
                integrationEvent.Description,
                integrationEvent.Location,
                integrationEvent.StartsAt,
                integrationEvent.EndsAt,
                integrationEvent.TicketTypes
                .Select(x => new TicketTypeRequest(x.Id, x.EventId, x.Name, x.Price, x.Currency, x.Quantity))
                .ToList());

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(CreateEventCommand), result.Error);
            }
        }
    }
}

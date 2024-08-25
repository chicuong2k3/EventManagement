
using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Exceptions;
using EventManagement.Events.IntegrationEvents;
using EventManagement.Ticketing.Application.Events.CancelEvent;
using MediatR;

namespace EventManagement.Ticketing.Application.IntegrationEventHandlers;

internal sealed class EventCancellationStartedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<EventCancellationStartedIntegrationEvent>
{
    public override async Task Handle(
        EventCancellationStartedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(new CancelEventCommand(integrationEvent.EventId), cancellationToken);

        if (result.IsFailure)
        {
            throw new InternalServerException(nameof(CancelEventCommand), result.Error);
        }
    }
}

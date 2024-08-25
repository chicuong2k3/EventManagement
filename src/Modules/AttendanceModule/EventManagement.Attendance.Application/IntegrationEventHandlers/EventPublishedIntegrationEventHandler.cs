
using EventManagement.Attendance.Application.Events;
using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Exceptions;
using EventManagement.Events.IntegrationEvents;
using MediatR;

namespace EventManagement.Attendance.Application.IntegrationEventHandlers;

internal sealed class EventPublishedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<EventPublishedIntegrationEvent>
{
    public override async Task Handle(
        EventPublishedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new CreateEventCommand(
                integrationEvent.EventId,
                integrationEvent.Title,
                integrationEvent.Description,
                integrationEvent.Location,
                integrationEvent.StartsAt,
                integrationEvent.EndsAt),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new InternalServerException(nameof(CreateEventCommand), result.Error);
        }
    }
}

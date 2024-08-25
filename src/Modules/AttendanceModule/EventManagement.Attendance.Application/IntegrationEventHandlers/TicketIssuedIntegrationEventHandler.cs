using EventManagement.Attendance.Application.Tickets.CreateTicket;
using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Exceptions;
using EventManagement.Ticketing.IntegrationEvents;
using MediatR;

namespace EventManagement.Attendance.Application.IntegrationEventHandlers;

internal sealed class TicketIssuedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<TicketIssuedIntegrationEvent>
{
    public override async Task Handle(
        TicketIssuedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new CreateTicketCommand(
                integrationEvent.TicketId,
                integrationEvent.CustomerId,
                integrationEvent.EventId,
                integrationEvent.Code),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new InternalServerException(nameof(CreateTicketCommand), result.Error);
        }
    }
}

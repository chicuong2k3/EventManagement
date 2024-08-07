using EventManagement.Events.Application.UseCases.TicketTypes;
using EventManagement.Events.PublicApi;
using MediatR;

namespace EventManagement.Events.Infrastructure.PublicApi;

public class EventsApi(ISender sender) : IEventsApi
{
    public async Task<GetTicketTypeResponse?> GetTicketTypeAsync(Guid ticketTypeId, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetTicketTypeByIdQuery(ticketTypeId), cancellationToken);

        if (result.IsFailure)
        {
            return null;
        }

        return new GetTicketTypeResponse(
            result.Value.Id,
            result.Value.EventId,
            result.Value.Name,
            result.Value.Price,
            result.Value.Currency,
            result.Value.Quantity
        );
    }
}


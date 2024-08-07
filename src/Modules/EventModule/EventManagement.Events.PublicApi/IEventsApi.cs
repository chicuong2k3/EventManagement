namespace EventManagement.Events.PublicApi;

public interface IEventsApi
{
    Task<GetTicketTypeResponse?> GetTicketTypeAsync(Guid ticketTypeId, CancellationToken cancellationToken = default);
}

public sealed record GetTicketTypeResponse(
    Guid Id,
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    int Quantity
);

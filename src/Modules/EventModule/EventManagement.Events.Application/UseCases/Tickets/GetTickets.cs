using Dapper;

namespace EventManagement.Events.Application.UseCases.Tickets;

public sealed record TicketResponse(
    Guid Id,
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    int Quantity);
public sealed record GetTicketsResponse(
    IReadOnlyCollection<TicketResponse> Data
);

public sealed record GetTicketsQuery(
    Guid EventId
) : IQuery<GetTicketsResponse>;

internal sealed class GetTicketsQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketsQuery, GetTicketsResponse>
{
    public async Task<Result<GetTicketsResponse>> Handle(GetTicketsQuery query, CancellationToken cancellationToken)
    {
        await using (var connection = await dbConnectionFactory.OpenConnectionAsync())
        {
            const string Sql =
               $"""
                SELECT
                    id AS {nameof(TicketResponse.Id)},
                    event_id AS {nameof(TicketResponse.EventId)},
                    name AS {nameof(TicketResponse.Name)},
                    price AS {nameof(TicketResponse.Price)},
                    currency AS {nameof(TicketResponse.Currency)},
                    quantity AS {nameof(TicketResponse.Quantity)}
                FROM events.tickets
                WHERE event_id = @EventId
                """;

            var result = (await connection.QueryAsync<TicketResponse>(Sql, query)).AsList();

            return new GetTicketsResponse(result);
        }
    }
}

using Dapper;
using EventManagement.Common.Domain;

namespace EventManagement.Events.Application.TicketTypes;

public sealed record TicketTypeResponse(
    Guid Id,
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    int Quantity);
public sealed record GetTicketTypesResponse(
    IReadOnlyCollection<TicketTypeResponse> Data
);

public sealed record GetTicketTypesQuery(
    Guid EventId
) : IQuery<GetTicketTypesResponse>;

internal sealed class GetTicketsQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketTypesQuery, GetTicketTypesResponse>
{
    public async Task<Result<GetTicketTypesResponse>> Handle(GetTicketTypesQuery query, CancellationToken cancellationToken)
    {
        await using (var connection = await dbConnectionFactory.OpenConnectionAsync())
        {
            const string Sql =
               $"""
                SELECT
                    id AS {nameof(TicketTypeResponse.Id)},
                    event_id AS {nameof(TicketTypeResponse.EventId)},
                    name AS {nameof(TicketTypeResponse.Name)},
                    price AS {nameof(TicketTypeResponse.Price)},
                    currency AS {nameof(TicketTypeResponse.Currency)},
                    quantity AS {nameof(TicketTypeResponse.Quantity)}
                FROM events.ticket_types
                WHERE event_id = @EventId
                """;

            var result = (await connection.QueryAsync<TicketTypeResponse>(Sql, query)).AsList();

            return new GetTicketTypesResponse(result);
        }
    }
}

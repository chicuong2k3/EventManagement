
using Dapper;

namespace EventManagement.Events.Application.UseCases.Tickets;

public sealed record GetTicketByIdResponse(
    Guid Id,
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    int Quantity
);

public sealed record GetTicketByIdQuery(Guid Id) : IQuery<GetTicketByIdResponse>;

internal sealed class GetTicketByIdQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketByIdQuery, GetTicketByIdResponse>
{
    public async Task<Result<GetTicketByIdResponse>> Handle(GetTicketByIdQuery query, CancellationToken cancellationToken)
    {
        await using (var connection = await dbConnectionFactory.OpenConnectionAsync())
        {
            const string Sql =
               $"""
                SELECT
                    id AS {nameof(GetTicketByIdResponse.Id)},
                    event_id AS {nameof(GetTicketByIdResponse.EventId)},
                    name AS {nameof(GetTicketByIdResponse.Name)},
                    price AS {nameof(GetTicketByIdResponse.Price)},
                    currency AS {nameof(GetTicketByIdResponse.Currency)},
                    quantity AS {nameof(GetTicketByIdResponse.Quantity)}
                FROM events.tickets
                WHERE id = @Id
                """
            ;

            var ticket = await connection.QuerySingleOrDefaultAsync<GetTicketByIdResponse>(Sql, query);
            if (ticket == null)
            {
                return Result.Failure<GetTicketByIdResponse>(TicketErrors.NotFound(query.Id));
            }

            return ticket;
        }
    }
}
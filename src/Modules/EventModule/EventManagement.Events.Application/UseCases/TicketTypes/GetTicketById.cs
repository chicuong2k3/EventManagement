
using Dapper;

namespace EventManagement.Events.Application.UseCases.TicketTypes;

public sealed record GetTicketTypeByIdResponse(
    Guid Id,
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    int Quantity
);

public sealed record GetTicketTypeByIdQuery(Guid Id) : IQuery<GetTicketTypeByIdResponse>;

internal sealed class GetTicketTypeByIdQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketTypeByIdQuery, GetTicketTypeByIdResponse>
{
    public async Task<Result<GetTicketTypeByIdResponse>> Handle(GetTicketTypeByIdQuery query, CancellationToken cancellationToken)
    {
        await using (var connection = await dbConnectionFactory.OpenConnectionAsync())
        {
            const string Sql =
               $"""
                SELECT
                    id AS {nameof(GetTicketTypeByIdResponse.Id)},
                    event_id AS {nameof(GetTicketTypeByIdResponse.EventId)},
                    name AS {nameof(GetTicketTypeByIdResponse.Name)},
                    price AS {nameof(GetTicketTypeByIdResponse.Price)},
                    currency AS {nameof(GetTicketTypeByIdResponse.Currency)},
                    quantity AS {nameof(GetTicketTypeByIdResponse.Quantity)}
                FROM events.ticket_types
                WHERE id = @Id
                """
            ;

            var ticketType = await connection.QueryFirstOrDefaultAsync<GetTicketTypeByIdResponse>(Sql, query);
            if (ticketType == null)
            {
                return Result.Failure<GetTicketTypeByIdResponse>(TicketTypeErrors.NotFound(query.Id));
            }

            return ticketType;
        }
    }
}
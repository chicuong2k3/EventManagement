
using Dapper;
using EventManagement.Ticketing.Domain.Tickets;

namespace EventManagement.Ticketing.Application.Tickets;

public sealed record GetTicketByIdResponse(
    Guid Id,
    Guid CustomerId,
    Guid OrderId,
    Guid EventId,
    Guid TicketTypeId,
    string Code,
    DateTime CreatedAt
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
                    customer_id AS {nameof(GetTicketByIdResponse.CustomerId)},
                    order_id AS {nameof(GetTicketByIdResponse.OrderId)},
                    event_id AS {nameof(GetTicketByIdResponse.EventId)},
                    ticket_type_id AS {nameof(GetTicketByIdResponse.TicketTypeId)},
                    code AS {nameof(GetTicketByIdResponse.Code)},
                    created_at_ AS {nameof(GetTicketByIdResponse.CreatedAt)}
                FROM ticketing.tickets
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
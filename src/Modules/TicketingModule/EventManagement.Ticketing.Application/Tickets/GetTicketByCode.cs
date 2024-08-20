
using Dapper;
using EventManagement.Ticketing.Domain.Tickets;

namespace EventManagement.Ticketing.Application.Tickets;

public sealed record GetTicketByCodeResponse(
    Guid Id,
    Guid CustomerId,
    Guid OrderId,
    Guid EventId,
    Guid TicketTypeId,
    string Code,
    DateTime CreatedAt
);

public sealed record GetTicketByCodeQuery(string Code) : IQuery<GetTicketByCodeResponse>;

internal sealed class GetTicketByCodeQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketByCodeQuery, GetTicketByCodeResponse>
{
    public async Task<Result<GetTicketByCodeResponse>> Handle(GetTicketByCodeQuery query, CancellationToken cancellationToken)
    {
        await using (var connection = await dbConnectionFactory.OpenConnectionAsync())
        {
            const string Sql =
                $"""
                SELECT
                    id AS {nameof(GetTicketByCodeResponse.Id)},
                    customer_id AS {nameof(GetTicketByCodeResponse.CustomerId)},
                    order_id AS {nameof(GetTicketByCodeResponse.OrderId)},
                    event_id AS {nameof(GetTicketByCodeResponse.EventId)},
                    ticket_type_id AS {nameof(GetTicketByCodeResponse.TicketTypeId)},
                    code AS {nameof(GetTicketByCodeResponse.Code)},
                    created_at_ AS {nameof(GetTicketByCodeResponse.CreatedAt)}
                FROM ticketing.tickets
                WHERE code = @Code
                """
            ;

            var ticket = await connection.QuerySingleOrDefaultAsync<GetTicketByCodeResponse>(Sql, query);
            if (ticket == null)
            {
                return Result.Failure<GetTicketByCodeResponse>(TicketErrors.NotFound(query.Code));
            }

            return ticket;
        }
    }
}
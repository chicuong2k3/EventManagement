
using Dapper;
using EventManagement.Common.Application.Data;
using EventManagement.Common.Domain;

namespace EventManagement.Ticketing.Application.Tickets;
public sealed record TicketResponse(
    Guid Id,
    Guid CustomerId,
    Guid OrderId,
    Guid EventId,
    Guid TicketTypeId,
    string Code,
    DateTime CreatedAt);

public sealed record GetTicketsForOrderResponse(
    IReadOnlyCollection<TicketResponse> Data
);

public sealed record GetTicketsForOrderQuery(Guid OrderId) : IQuery<GetTicketsForOrderResponse>;

internal sealed class GetTicketForOrderQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketsForOrderQuery, GetTicketsForOrderResponse>
{
    public async Task<Result<GetTicketsForOrderResponse>> Handle(GetTicketsForOrderQuery query, CancellationToken cancellationToken)
    {
        await using (var connection = await dbConnectionFactory.OpenConnectionAsync())
        {
            const string Sql =
                $"""
                SELECT
                    id AS {nameof(TicketResponse.Id)},
                    customer_id AS {nameof(TicketResponse.CustomerId)},
                    order_id AS {nameof(TicketResponse.OrderId)},
                    event_id AS {nameof(TicketResponse.EventId)},
                    ticket_type_id AS {nameof(TicketResponse.TicketTypeId)},
                    code AS {nameof(TicketResponse.Code)},
                    created_at AS {nameof(TicketResponse.CreatedAt)}
                FROM ticketing.tickets
                WHERE order_id = @OrderId
                """
            ;

            var tickets = (await connection.QueryAsync<TicketResponse>(Sql, query)).AsList();

            return new GetTicketsForOrderResponse(tickets);
        }
    }
}
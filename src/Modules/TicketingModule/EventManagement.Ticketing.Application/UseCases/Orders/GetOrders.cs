
using Dapper;
using EventManagement.Common.Application.Data;

namespace EventManagement.Ticketing.Application.UseCases.Orders;
public sealed record OrderResponse(
    Guid Id,
    Guid CustomerId,
    OrderStatus Status,
    decimal TotalPrice,
    DateTime CreatedAt);

public sealed record GetOrdersResponse(IReadOnlyCollection<OrderResponse> Data);
public sealed record GetOrdersQuery(Guid CustomerId) : IQuery<GetOrdersResponse>;
internal sealed class GetOrdersQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetOrdersQuery, GetOrdersResponse>
{
    public async Task<Result<GetOrdersResponse>> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        await using (var connection = await dbConnectionFactory.OpenConnectionAsync())
        {
            const string Sql =
            $"""
             SELECT
                 id AS {nameof(OrderResponse.Id)},
                 customer_id AS {nameof(OrderResponse.CustomerId)},
                 status AS {nameof(OrderResponse.Status)},
                 total_price AS {nameof(OrderResponse.TotalPrice)},
                 created_at AS {nameof(OrderResponse.CreatedAt)}
             FROM ticketing.orders
             WHERE customer_id = @CustomerId
             """;

            var orders = (await connection.QueryAsync<OrderResponse>(Sql, query)).AsList();

            return new GetOrdersResponse(orders);
        }
        

    }
}

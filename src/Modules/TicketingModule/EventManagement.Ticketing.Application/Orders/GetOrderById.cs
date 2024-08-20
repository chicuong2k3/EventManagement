
using Dapper;
using EventManagement.Ticketing.Domain.Orders;

namespace EventManagement.Ticketing.Application.Orders;
public sealed record OrderItemResponse(
    Guid OrderItemId,
    Guid OrderId,
    Guid TicketTypeId,
    int Quantity,
    decimal UnitPrice,
    decimal Price,
    string Currency);
public sealed record GetOrderByIdResponse(
    Guid Id,
    Guid CustomerId,
    OrderStatus Status,
    decimal TotalPrice,
    DateTime CreatedAt)
{
    public List<OrderItemResponse> OrderItems { get; } = [];
}
public sealed record GetOrderByIdQuery(Guid Id) : IQuery<GetOrderByIdResponse>;
internal sealed class GetOrderByIdQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetOrderByIdQuery, GetOrderByIdResponse>
{
    public async Task<Result<GetOrderByIdResponse>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        await using (var connection = await dbConnectionFactory.OpenConnectionAsync())
        {
            const string Sql =
            $"""
             SELECT
                 o.id AS {nameof(GetOrderByIdResponse.Id)},
                 o.customer_id AS {nameof(GetOrderByIdResponse.CustomerId)},
                 o.status AS {nameof(GetOrderByIdResponse.Status)},
                 o.total_price AS {nameof(GetOrderByIdResponse.TotalPrice)},
                 o.created_at AS {nameof(GetOrderByIdResponse.CreatedAt)},
                 oi.id AS {nameof(OrderItemResponse.OrderItemId)},
                 oi.order_id AS {nameof(OrderItemResponse.OrderId)},
                 oi.ticket_type_id AS {nameof(OrderItemResponse.TicketTypeId)},
                 oi.quantity AS {nameof(OrderItemResponse.Quantity)},
                 oi.unit_price AS {nameof(OrderItemResponse.UnitPrice)},
                 oi.price AS {nameof(OrderItemResponse.Price)},
                 oi.currency AS {nameof(OrderItemResponse.Currency)}
             FROM ticketing.orders o
             JOIN ticketing.order_items oi ON oi.order_id = o.id
             WHERE o.id = @Id
             """;

            Dictionary<Guid, GetOrderByIdResponse> ordersDictionary = [];
            await connection.QueryAsync<GetOrderByIdResponse, OrderItemResponse, GetOrderByIdResponse>(
                Sql,
                (order, orderItem) =>
                {
                    if (ordersDictionary.TryGetValue(order.Id, out GetOrderByIdResponse? existingEvent))
                    {
                        order = existingEvent;
                    }
                    else
                    {
                        ordersDictionary.Add(order.Id, order);
                    }

                    order.OrderItems.Add(orderItem);

                    return order;
                },
                query,
                splitOn: nameof(OrderItemResponse.OrderItemId));

            if (!ordersDictionary.TryGetValue(query.Id, out GetOrderByIdResponse? orderResponse))
            {
                return Result.Failure<GetOrderByIdResponse>(OrderErrors.NotFound(query.Id));
            }

            return orderResponse;
        }


    }
}

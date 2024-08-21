using EventManagement.Common.Application.EventBuses;

namespace EventManagement.Ticketing.IntegrationEvents
{
    public sealed class OrderCreatedIntegrationEvent : IntegrationEvent
    {
        public OrderCreatedIntegrationEvent(
            Guid eventId,
            DateTime occurredOn,
            Guid orderId,
            Guid customerId,
            decimal totalPrice,
            DateTime createdAt,
            List<OrderItemModel> orderItems
            ) : base(eventId, occurredOn)
        {
            OrderId = orderId;
            CustomerId = customerId;
            TotalPrice = totalPrice;
            CreatedAt = createdAt;
            OrderItems = orderItems;
        }
        public Guid OrderId { get; init; }
        public Guid CustomerId { get; init; }
        public decimal TotalPrice { get; init; }
        public string Currency { get; init; }
        public DateTime CreatedAt { get; init; }
        public List<OrderItemModel> OrderItems { get; init; }
    }

    public class OrderItemModel
    {
        public Guid Id { get; init; }
        public Guid OrderId { get; init; }
        public Guid TicketTypeId { get; init; }
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }
        public decimal Price { get; init; }
        public string Currency { get; init; }
    }
}

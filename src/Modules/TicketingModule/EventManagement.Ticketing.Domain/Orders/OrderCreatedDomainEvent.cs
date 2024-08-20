namespace EventManagement.Ticketing.Domain.Orders
{
    public sealed class OrderCreatedDomainEvent(Guid orderId) : DomainEvent
    {
        public Guid OrderId { get; init; } = orderId;
    }

}

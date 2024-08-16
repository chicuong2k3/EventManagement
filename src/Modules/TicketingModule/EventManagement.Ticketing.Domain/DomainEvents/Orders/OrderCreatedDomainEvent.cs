namespace EventManagement.Ticketing.Domain.DomainEvents.Orders
{
    public sealed class OrderCreatedDomainEvent(Guid orderId) : DomainEventBase
    {
        public Guid OrderId { get; init; } = orderId;
    }

}

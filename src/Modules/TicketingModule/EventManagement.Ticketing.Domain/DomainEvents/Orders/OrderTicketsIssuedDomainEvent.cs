namespace EventManagement.Ticketing.Domain.DomainEvents.Orders
{
    public sealed class OrderTicketsIssuedDomainEvent(Guid orderId) : DomainEventBase
    {
        public Guid OrderId { get; init; } = orderId;
    }
}

namespace EventManagement.Ticketing.Domain.DomainEvents.Payments
{
    public sealed class PaymentCreatedDomainEvent(Guid paymentId) : DomainEventBase
    {
        public Guid PaymentId { get; init; } = paymentId;
    }
}

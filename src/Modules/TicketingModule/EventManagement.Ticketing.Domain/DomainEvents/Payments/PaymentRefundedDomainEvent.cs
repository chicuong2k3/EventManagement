namespace EventManagement.Ticketing.Domain.DomainEvents.Payments
{
    public sealed class PaymentRefundedDomainEvent(
        Guid paymentId, 
        Guid transactionId, 
        decimal refundAmount)
        : DomainEventBase
    {
        public Guid PaymentId { get; init; } = paymentId;
        public Guid TransactionId { get; init; } = transactionId;
        public decimal RefundAmount { get; init; } = refundAmount;
    }
}

using EventManagement.Ticketing.Domain.Payments;

namespace EventManagement.Ticketing.Application.Payments.RefundPayment;

internal sealed class PaymentRefundedDomainEventHandler(IPaymentService paymentService)
    : DomainEventHandler<PaymentRefundedDomainEvent>
{
    public override async Task Handle(
        PaymentRefundedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await paymentService.RefundAsync(domainEvent.TransactionId, domainEvent.RefundAmount);
    }
}

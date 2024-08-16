namespace EventManagement.Ticketing.Application.Services;
public sealed record PaymentResponse(Guid TransactionId, decimal Amount, string Currency);
public interface IPaymentService
{
    Task<PaymentResponse> ChargeAsync(decimal amount, string currency);
    Task RefundAsync(Guid transactionId, decimal amount);
}

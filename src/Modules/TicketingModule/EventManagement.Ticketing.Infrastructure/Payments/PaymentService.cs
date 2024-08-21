﻿using EventManagement.Ticketing.Application.Services;

namespace EventManagement.Ticketing.Infrastructure.Payments
{
    internal sealed class PaymentService : IPaymentService
    {
        public Task<PaymentResponse> ChargeAsync(decimal amount, string currency)
        {
            return Task.FromResult(new PaymentResponse(Guid.NewGuid(), amount, currency));
        }

        public Task RefundAsync(Guid transactionId, decimal amount)
        {
            return Task.CompletedTask;
        }
    }

}
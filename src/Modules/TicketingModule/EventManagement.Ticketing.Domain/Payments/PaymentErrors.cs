namespace EventManagement.Ticketing.Domain.Payments
{
    public static class PaymentErrors
    {
        public static Error NotFound(Guid paymentId) =>
            Error.NotFound("Payment.NotFound", $"The payment with the identifier {paymentId} was not found");

        public static readonly Error AlreadyRefunded =
            Error.Problem("Payment.AlreadyRefunded", "The payment was already refunded");

        public static readonly Error NotEnoughFunds =
            Error.Problem("Payment.NotEnoughFunds", "There are not enough funds for a refund");
    }

}

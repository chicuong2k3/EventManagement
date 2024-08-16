namespace EventManagement.Ticketing.Domain.Errors
{
    public static class OrderErrors
    {
        public static Error NotFound(Guid orderId) =>
            Error.NotFound("Order.NotFound", $"The order with the identifier {orderId} was not found");


        public static readonly Error TicketsAlreadyIssues = Error.Problem(
            "Order.TicketsAlreadyIssued",
            "The tickets for this order were already issued");
    }
}

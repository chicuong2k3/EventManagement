namespace EventManagement.Ticketing.Domain.Customers
{
    public static class CustomerErrors
    {
        public static Error NotFound(Guid customerId) =>
            Error.NotFound("Customer.NotFound", $"The customer with the identifier {customerId} was not found");
    }
}

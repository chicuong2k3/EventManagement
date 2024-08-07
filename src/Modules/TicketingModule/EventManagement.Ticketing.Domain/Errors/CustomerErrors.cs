using EventManagement.Common.Domain.Results;

namespace EventManagement.Ticketing.Domain.Errors
{
    public static class CustomerErrors
    {
        public static Error NotFound(Guid customerId) =>
            Error.NotFound("Customer.NotFound", $"The customer with the identifier {customerId} was not found");
    }
}

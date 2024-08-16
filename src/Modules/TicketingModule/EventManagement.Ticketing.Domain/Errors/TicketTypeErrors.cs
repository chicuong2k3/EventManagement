using EventManagement.Common.Domain.Results;

namespace EventManagement.Ticketing.Domain.Errors
{
    public static class TicketTypeErrors
    {
        public static Error NotFound(Guid ticketId) =>
            Error.NotFound("TicketType.NotFound", $"The ticket type with the identifier {ticketId} was not found");
        public static Error NotEnoughQuantity(decimal availableQuantity) =>
        Error.Problem(
            "TicketType.NotEnoughQuantity",
            $"The ticket type has {availableQuantity} quantity available");
    }
}

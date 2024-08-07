using EventManagement.Common.Domain.Results;

namespace EventManagement.Ticketing.Domain.Errors
{
    public static class TicketTypeErrors
    {
        public static Error NotFound(Guid ticketId) =>
            Error.NotFound("TicketType.NotFound", $"The ticket type with the identifier {ticketId} was not found");
    }
}

namespace EventManagement.Events.Domain.TicketTypes
{
    public static class TicketTypeErrors
    {
        public static Error NotFound(Guid ticketId) => Error.NotFound(
            "TicketType.NotFound",
            $"The ticket type with the identifier {ticketId} was not found");
    }
}

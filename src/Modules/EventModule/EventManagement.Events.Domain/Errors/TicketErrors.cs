namespace EventManagement.Events.Domain.Errors
{
    public static class TicketErrors
    {
        public static Error NotFound(Guid ticketId) => Error.NotFound(
            "Ticket.NotFound",
            $"The ticket with the identifier {ticketId} was not found");
    }
}

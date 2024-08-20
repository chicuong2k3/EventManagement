namespace EventManagement.Ticketing.Domain.Tickets
{
    public static class TicketErrors
    {
        public static Error NotFound(Guid ticketId) =>
            Error.NotFound("Ticket.NotFound", $"The ticket with the identifier {ticketId} was not found");

        public static Error NotFound(string code) =>
            Error.NotFound("Ticket.NotFound", $"The ticket with the code {code} was not found");
    }
}

namespace EventManagement.Ticketing.Domain.Tickets
{
    public class TicketCreatedDomainEvent(Guid ticketId) : DomainEvent
    {
        public Guid TicketId { get; init; } = ticketId;
    }
}

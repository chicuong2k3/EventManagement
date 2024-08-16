namespace EventManagement.Ticketing.Domain.DomainEvents.Tickets
{
    public class TicketCreatedDomainEvent(Guid ticketId) : DomainEventBase
    {
        public Guid TicketId { get; init; } = ticketId;
    }
}

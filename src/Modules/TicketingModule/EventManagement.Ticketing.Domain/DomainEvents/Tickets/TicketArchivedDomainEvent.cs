namespace EventManagement.Ticketing.Domain.DomainEvents.Tickets
{
    public sealed class TicketArchivedDomainEvent(Guid ticketId, string code) : DomainEventBase
    {
        public Guid TicketId { get; init; } = ticketId;
        public string Code { get; init; } = code;
    }

}

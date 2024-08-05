
namespace EventManagement.Events.Domain.DomainEvents.Tickets
{
    public sealed class TicketCreated(Guid ticketId) : DomainEventBase
    {
        public Guid TicketId { get; init; } = ticketId;
    }
}

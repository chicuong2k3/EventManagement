
namespace EventManagement.Ticketing.Domain.DomainEvents.TicketTypes
{
    public sealed class TicketTypeSoldOutDomainEvent(Guid ticketTypeId) : DomainEventBase
    {
        public Guid TicketTypeId { get; init; } = ticketTypeId;
    }
}

namespace EventManagement.Events.Domain.DomainEvents.TicketTypes
{
    public sealed class TicketTypeCreatedDomainEvent(Guid ticketTypeId) : DomainEventBase
    {
        public Guid TicketTypeId { get; init; } = ticketTypeId;
    }
}

namespace EventManagement.Events.Domain.DomainEvents.TicketTypes
{
    public sealed class TicketTypePriceChangedDomainEvent(Guid ticketTypeId, decimal price) : DomainEventBase
    {
        public Guid TicketId { get; init; } = ticketTypeId;

        public decimal Price { get; init; } = price;
    }
}

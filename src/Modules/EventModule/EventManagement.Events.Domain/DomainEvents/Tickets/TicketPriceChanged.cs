
namespace EventManagement.Events.Domain.DomainEvents.Tickets
{
    public sealed class TicketPriceChanged(Guid ticketId, decimal price) : DomainEventBase
    {
        public Guid TicketId { get; init; } = ticketId;

        public decimal Price { get; init; } = price;
    }
}

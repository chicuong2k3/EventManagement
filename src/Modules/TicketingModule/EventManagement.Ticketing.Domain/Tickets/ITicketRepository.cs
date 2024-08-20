using EventManagement.Ticketing.Domain.Events;

namespace EventManagement.Ticketing.Domain.Tickets
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetForEventAsync(EventEntity eventEntity, CancellationToken cancellationToken = default);

        void InsertRange(IEnumerable<Ticket> tickets);
    }
}

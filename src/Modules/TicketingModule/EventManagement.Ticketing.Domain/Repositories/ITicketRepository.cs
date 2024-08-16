namespace EventManagement.Ticketing.Domain.Repositories
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetForEventAsync(EventEntity eventEntity, CancellationToken cancellationToken = default);

        void InsertRange(IEnumerable<Ticket> tickets);
    }
}

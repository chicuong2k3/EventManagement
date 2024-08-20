namespace EventManagement.Ticketing.Domain.TicketTypes
{
    public interface ITicketTypeRepository
    {
        Task<TicketType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TicketType?> GetByIdWithLockAsync(Guid id, CancellationToken cancellationToken = default);
        void Insert(TicketType ticket);
        void InsertRange(IEnumerable<TicketType> ticketTypes);
    }
}

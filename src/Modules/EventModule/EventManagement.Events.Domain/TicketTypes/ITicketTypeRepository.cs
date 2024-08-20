namespace EventManagement.Events.Domain.TicketTypes
{
    public interface ITicketTypeRepository
    {
        Task<TicketType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken = default);

        void Insert(TicketType ticketType);
    }
}

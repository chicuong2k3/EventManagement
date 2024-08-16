namespace EventManagement.Ticketing.Domain.Repositories
{
    public interface IEventRepository
    {
        Task<EventEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        void Insert(EventEntity eventEntity);
    }
}

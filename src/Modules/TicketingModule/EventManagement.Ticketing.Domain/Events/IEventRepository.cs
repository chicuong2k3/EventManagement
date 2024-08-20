namespace EventManagement.Ticketing.Domain.Events
{
    public interface IEventRepository
    {
        Task<EventEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        void Insert(EventEntity eventEntity);
    }
}

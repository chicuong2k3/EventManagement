using EventManagement.Events.Domain.Entities;

namespace EventManagement.Events.Domain.Repositories
{
    public interface IEventRepository
    {
        Task<EventEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        void Insert(EventEntity eventEntity);
    }
}

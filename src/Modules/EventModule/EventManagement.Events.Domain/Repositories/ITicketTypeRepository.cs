
using EventManagement.Events.Domain.Entities;

namespace EventManagement.Events.Domain.Repositories
{
    public interface ITicketTypeRepository
    {
        Task<TicketType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken = default);

        void Insert(TicketType ticket);
    }
}

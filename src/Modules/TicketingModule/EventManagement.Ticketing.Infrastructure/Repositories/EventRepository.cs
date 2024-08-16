

using EventManagement.Ticketing.Infrastructure.Data;

namespace EventManagement.Ticketing.Infrastructure.Repositories
{
    internal sealed class EventRepository(TicketingDbContext dbContext) : IEventRepository
    {
        public async Task<EventEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Events.FindAsync(id, cancellationToken);
        }

        public void Insert(EventEntity eventEntity)
        {
            dbContext.Events.Add(eventEntity);
        }
    }
}

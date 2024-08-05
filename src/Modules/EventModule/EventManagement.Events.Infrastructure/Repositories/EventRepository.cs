
namespace EventManagement.Events.Infrastructure.Repositories
{
    internal sealed class EventRepository(AppDbContext dbContext) : IEventRepository
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

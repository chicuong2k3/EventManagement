using EventManagement.Events.Domain.TicketTypes;

namespace EventManagement.Events.Infrastructure.TicketTypes
{
    internal sealed class TicketTypeRepository(EventsDbContext dbContext) : ITicketTypeRepository
    {
        public async Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken = default)
        {
            return await dbContext.TicketTypes.AnyAsync(x => x.EventId == eventId, cancellationToken);
        }

        public async Task<TicketType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.TicketTypes.FindAsync(id, cancellationToken);
        }

        public void Insert(TicketType ticketType)
        {
            dbContext.TicketTypes.Add(ticketType);
        }
    }
}

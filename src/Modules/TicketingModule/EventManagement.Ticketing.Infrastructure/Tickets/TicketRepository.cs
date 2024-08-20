using EventManagement.Ticketing.Domain.Events;
using EventManagement.Ticketing.Domain.Tickets;
using EventManagement.Ticketing.Infrastructure.Data;

namespace EventManagement.Ticketing.Infrastructure.Tickets
{
    internal sealed class TicketRepository(TicketingDbContext dbContext) : ITicketRepository
    {
        public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Tickets.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Ticket>> GetForEventAsync(EventEntity eventEntity, CancellationToken cancellationToken = default)
        {
            return await dbContext.Tickets
                .Where(t => t.EventId == eventEntity.Id)
                .ToListAsync(cancellationToken);
        }

        public void InsertRange(IEnumerable<Ticket> tickets)
        {
            dbContext.Tickets.AddRange(tickets);
        }
    }
}

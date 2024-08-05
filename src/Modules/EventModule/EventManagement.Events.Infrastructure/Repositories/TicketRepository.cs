namespace EventManagement.Events.Infrastructure.Repositories
{
    internal sealed class TicketRepository(AppDbContext dbContext) : ITicketRepository
    {
        public async Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Tickets.AnyAsync(x => x.EventId == eventId, cancellationToken);
        }

        public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Tickets.FindAsync(id, cancellationToken);
        }

        public void Insert(Ticket ticket)
        {
            dbContext.Tickets.Add(ticket);
        }
    }
}

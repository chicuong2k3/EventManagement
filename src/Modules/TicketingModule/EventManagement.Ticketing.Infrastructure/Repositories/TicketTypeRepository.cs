using EventManagement.Ticketing.Infrastructure.Data;

namespace EventManagement.Ticketing.Infrastructure.Repositories
{
    internal sealed class TicketTypeRepository(TicketingDbContext dbContext) : ITicketTypeRepository
    {
        public async Task<TicketType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.TicketTypes.FindAsync(id, cancellationToken);
        }

        public async Task<TicketType?> GetByIdWithLockAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext
            .TicketTypes
            .FromSql(
                $"""
                SELECT id, event_id, name, price, currency, quantity, available_quantity
                FROM ticketing.ticket_types
                WHERE id = {id}
                FOR UPDATE NOWAIT
                """)
            .SingleOrDefaultAsync(cancellationToken);
        }

        public void InsertRange(IEnumerable<TicketType> ticketTypes)
        {
            dbContext.TicketTypes.AddRange(ticketTypes);
        }
    }
}

using EventManagement.Ticketing.Domain.Orders;
using EventManagement.Ticketing.Infrastructure.Data;

namespace EventManagement.Ticketing.Infrastructure.Orders
{
    internal sealed class OrderRepository(TicketingDbContext dbContext) : IOrderRepository
    {
        public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Orders.FindAsync(id, cancellationToken);
        }

        public void Insert(Order order)
        {
            dbContext.Orders.Add(order);
        }
    }
}

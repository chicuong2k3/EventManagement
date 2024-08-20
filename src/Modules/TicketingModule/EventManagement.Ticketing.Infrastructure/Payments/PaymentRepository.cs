using EventManagement.Ticketing.Domain.Events;
using EventManagement.Ticketing.Domain.Payments;
using EventManagement.Ticketing.Infrastructure.Data;
using MassTransit;

namespace EventManagement.Ticketing.Infrastructure.Payments
{
    internal sealed class PaymentRepository(TicketingDbContext dbContext) : IPaymentRepository
    {
        public async Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Payments.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Payment>> GetForEventAsync(EventEntity eventEntity, CancellationToken cancellationToken = default)
        {
            return await (
            from order in dbContext.Orders
            join payment in dbContext.Payments on order.Id equals payment.OrderId
            join orderItem in dbContext.OrderItems on order.Id equals orderItem.OrderId
            join ticketType in dbContext.TicketTypes on orderItem.TicketTypeId equals ticketType.Id
            where ticketType.EventId == eventEntity.Id
            select payment).ToListAsync(cancellationToken);
        }

        public void Insert(Payment payment)
        {
            dbContext.Payments.Add(payment);
        }
    }
}

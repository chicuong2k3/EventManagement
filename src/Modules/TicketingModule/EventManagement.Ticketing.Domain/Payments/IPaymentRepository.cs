using EventManagement.Ticketing.Domain.Events;

namespace EventManagement.Ticketing.Domain.Payments
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Payment>> GetForEventAsync(EventEntity eventEntity, CancellationToken cancellationToken = default);
        void Insert(Payment payment);
    }
}

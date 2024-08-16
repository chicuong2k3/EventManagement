namespace EventManagement.Ticketing.Domain.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Payment>> GetForEventAsync(EventEntity eventEntity, CancellationToken cancellationToken = default);
        void Insert(Payment payment);
    }
}

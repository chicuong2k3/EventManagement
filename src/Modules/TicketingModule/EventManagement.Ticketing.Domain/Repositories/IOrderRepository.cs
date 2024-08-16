namespace EventManagement.Ticketing.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        void Insert(Order order);
    }
}

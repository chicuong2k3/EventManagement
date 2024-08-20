namespace EventManagement.Ticketing.Domain.Orders
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        void Insert(Order order);
    }
}

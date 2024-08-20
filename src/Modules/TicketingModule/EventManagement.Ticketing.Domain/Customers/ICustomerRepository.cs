namespace EventManagement.Ticketing.Domain.Customers
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Insert(Customer customer);
    }
}

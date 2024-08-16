namespace EventManagement.Ticketing.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Insert(Customer customer);
    }
}

﻿

using EventManagement.Ticketing.Infrastructure.Data;

namespace EventManagement.Ticketing.Infrastructure.Repositories
{
    internal sealed class CustomerRepository(TicketingDbContext dbContext) : ICustomerRepository
    {
        public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Customers.FindAsync(id, cancellationToken);
        }

        public void Insert(Customer customer)
        {
            dbContext.Customers.Add(customer);
        }
    }
}
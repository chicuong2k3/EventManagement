using System.Data.Common;
using EventManagement.Ticketing.Application.Abstractions.Data;
using EventManagement.Ticketing.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventManagement.Ticketing.Infrastructure.Data;

public sealed class TicketingDbContext(DbContextOptions<TicketingDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Ticketing);

        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    }

    public async Task<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction != null)
        {
            await Database.CurrentTransaction.DisposeAsync();
        }

        return (await Database.BeginTransactionAsync(cancellationToken)).GetDbTransaction();
    }
    
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
    }
}

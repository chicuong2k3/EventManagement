using System.Data.Common;
using System.Reflection;
using EventManagement.Common.Infrastructure.Outbox;
using EventManagement.Ticketing.Application.Abstractions.Data;
using EventManagement.Ticketing.Domain.Customers;
using EventManagement.Ticketing.Domain.Events;
using EventManagement.Ticketing.Domain.Orders;
using EventManagement.Ticketing.Domain.Payments;
using EventManagement.Ticketing.Domain.Tickets;
using EventManagement.Ticketing.Domain.TicketTypes;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventManagement.Ticketing.Infrastructure.Data;

public sealed class TicketingDbContext(DbContextOptions<TicketingDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<Customer> Customers { get; set; }
    internal DbSet<EventEntity> Events { get; set; }

    internal DbSet<TicketType> TicketTypes { get; set; }

    internal DbSet<Order> Orders { get; set; }

    internal DbSet<OrderItem> OrderItems { get; set; }

    internal DbSet<Ticket> Tickets { get; set; }

    internal DbSet<Payment> Payments { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Ticketing);

        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async Task<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction != null)
        {
            await Database.CurrentTransaction.DisposeAsync();
        }

        return (await Database.BeginTransactionAsync(cancellationToken)).GetDbTransaction();
    }

}

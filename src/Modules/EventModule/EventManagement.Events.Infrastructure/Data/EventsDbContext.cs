using EventManagement.Common.Infrastructure.Inbox;
using EventManagement.Common.Infrastructure.Outbox;
using EventManagement.Events.Application.Abstractions.Data;
using EventManagement.Events.Domain.Categories;
using EventManagement.Events.Domain.Events;
using EventManagement.Events.Domain.TicketTypes;


namespace EventManagement.Events.Infrastructure.Data
{
    public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options)
        : DbContext(options), IUnitOfWork
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(Schemas.Events);

            modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());

            modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
            modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        internal DbSet<EventEntity> Events { get; set; }
        internal DbSet<Category> Categories { get; set; }
        internal DbSet<TicketType> TicketTypes { get; set; }

        
    }

}

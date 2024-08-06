using EventManagement.Events.Application.Abstractions.Data;


namespace EventManagement.Events.Infrastructure.Data
{
    public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options)
        : DbContext(options), IUnitOfWork
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(Schemas.Events);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await SaveChangesAsync(cancellationToken);
        }

        internal DbSet<EventEntity> Events { get; set; }
        internal DbSet<Category> Categories { get; set; }
        internal DbSet<Ticket> Tickets { get; set; }
    }

}

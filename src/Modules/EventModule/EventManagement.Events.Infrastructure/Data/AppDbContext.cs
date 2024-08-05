using EventManagement.Events.Application.Abstractions;


namespace EventManagement.Events.Infrastructure.Data
{
    public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
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

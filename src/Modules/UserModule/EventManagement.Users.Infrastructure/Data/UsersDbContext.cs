

namespace EventManagement.Users.Infrastructure.Data
{
    public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options)
        : DbContext(options), IUnitOfWork
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(Schemas.Users);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await SaveChangesAsync(cancellationToken);
        }

        internal DbSet<User> Users { get; set; }
    }

}

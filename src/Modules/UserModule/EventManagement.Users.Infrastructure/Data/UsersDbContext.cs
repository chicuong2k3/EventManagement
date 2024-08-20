using EventManagement.Common.Infrastructure.Outbox;
using EventManagement.Users.Application.Abstractions.Data;
using EventManagement.Users.Domain.Users;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace EventManagement.Users.Infrastructure.Data
{
    public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options)
        : DbContext(options), IUnitOfWork
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(Schemas.Users);

            modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        internal DbSet<User> Users { get; set; }
        internal DbSet<Role> Roles { get; set; }
        internal DbSet<Permission> Permissions { get; set; }


    }

}

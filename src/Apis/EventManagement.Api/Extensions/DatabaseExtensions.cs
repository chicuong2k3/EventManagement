using Microsoft.EntityFrameworkCore;
using EventManagement.Events.Infrastructure.Data;
using EventManagement.Users.Infrastructure.Data;
using EventManagement.Ticketing.Infrastructure.Data;

namespace EventManagement.Api.Extensions
{
    public static class DatabaseExtensions
    {
        public static void ApplyMigrations(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                ApplyMigration<EventsDbContext>(scope);
                ApplyMigration<UsersDbContext>(scope);
                ApplyMigration<TicketingDbContext>(scope);
            }
        }

        internal static void ApplyMigration<TDbContext>(IServiceScope scope)
            where TDbContext : DbContext
        {
            using (var context = scope.ServiceProvider.GetRequiredService<TDbContext>())
            {
                context.Database.Migrate();
            }
        }
    }
}

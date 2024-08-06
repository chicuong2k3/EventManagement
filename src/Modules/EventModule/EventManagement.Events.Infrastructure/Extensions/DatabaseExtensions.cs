using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Events.Infrastructure.Extensions
{
    public static class DatabaseExtensions
    {
        public static void ApplyMigrations(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                ApplyMigration<AppDbContext>(scope);
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

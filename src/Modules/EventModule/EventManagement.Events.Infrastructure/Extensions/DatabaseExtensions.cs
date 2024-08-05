using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Events.Infrastructure.Extensions
{
    public static class DatabaseExtensions
    {
        internal static void ApplyMigrations(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
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

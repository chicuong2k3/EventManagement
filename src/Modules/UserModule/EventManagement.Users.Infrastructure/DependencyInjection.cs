using EventManagement.Common.Infrastructure.Interceptors;
using EventManagement.Users.Application.Abstractions.Identity;
using EventManagement.Users.Infrastructure.Authorization;
using EventManagement.Users.Infrastructure.Identity;
using EventManagement.Users.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EventManagement.Users.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUsersInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // KeyCloak
            services.Configure<KeyCloakOptions>(configuration.GetSection("Users:KeyCloak"));

            services.AddTransient<KeyCloakAuthDelegatingHandler>();

            services.AddHttpClient<KeyCloakClient>((serviceProvider, httpClient) =>
            {
                var keyCloakOptions = serviceProvider
                    .GetRequiredService<IOptions<KeyCloakOptions>>().Value;

                httpClient.BaseAddress = new Uri(keyCloakOptions.AdminUrl);
            })
            .AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>();

            services.AddTransient<IIdentityProviderService, IdentityProviderService>();

            // Authorization
            services.AddScoped<IPermissionService, PermissionService>();

            // Entity Framework Core
            string dbConnectionString = configuration.GetConnectionString("Database")!;
            services.AddDbContext<UsersDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(dbConnectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Users);
                })
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(serviceProvider.GetRequiredService<PublishDomainEventsInterceptor>());
            });

            services.AddScoped<IUserRepository, UserRepository>();


            services.AddScoped<IUnitOfWork>(serviceProvider =>
            {
                return serviceProvider.GetRequiredService<UsersDbContext>();
            });



            return services;
        }
    }
}

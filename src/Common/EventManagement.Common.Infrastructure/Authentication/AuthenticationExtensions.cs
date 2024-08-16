using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Common.Infrastructure.Authentication
{
    internal static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationInfrastructure(this IServiceCollection services)
        {
            services.AddAuthentication().AddJwtBearer();

            services.AddHttpContextAccessor();

            services.ConfigureOptions<JwtBearerConfigureOptions>();

            services.AddAuthorization();

            return services;
        }
    }
}

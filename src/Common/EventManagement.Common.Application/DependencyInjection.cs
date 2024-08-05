using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EventManagement.Common.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationCommon(
            this IServiceCollection services,
            Assembly[] assemblies)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(assemblies);
                //config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
                //config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            });

            services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);

            return services;
        }
    }
}

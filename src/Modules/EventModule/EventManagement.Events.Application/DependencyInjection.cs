using EventManagement.Common.Application;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EventManagement.Events.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services,
            Assembly[] assemblies)
        {
            services.AddApplicationCommon(assemblies);

            return services;
        }
    }
}

using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;

namespace EventManagement.Common.Infrastructure.Inbox
{
    public static class IntegrationEventHandlerFactory
    {
        private static readonly ConcurrentDictionary<string, Type[]> HandlersDictionary = new();

        public static IEnumerable<IIntegrationEventHandler> GetHandlers(
            Type type,
            IServiceProvider serviceProvider,
            Assembly assembly)
        {

            var integrationEventHandlerTypes = HandlersDictionary.GetOrAdd(
                $"{assembly.GetName().Name}{type.Name}",
                _ =>
                {
                    var integrationEventHandlerTypes = assembly
                        .GetTypes()
                        .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler<>).MakeGenericType(type)))
                        .ToArray();

                    return integrationEventHandlerTypes;
                });

            List<IIntegrationEventHandler> handlers = [];

            foreach (var integrationEventHandlerType in integrationEventHandlerTypes)
            {
                var handler = serviceProvider.GetRequiredService(integrationEventHandlerType);
                handlers.Add((handler as IIntegrationEventHandler)!);
            }

            return handlers;
        }
    }
}

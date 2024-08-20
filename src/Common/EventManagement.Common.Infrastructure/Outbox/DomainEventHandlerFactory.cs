using EventManagement.Common.Application.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;

namespace EventManagement.Common.Infrastructure.Outbox
{
    public static class DomainEventHandlerFactory
    {
        private static readonly ConcurrentDictionary<string, Type[]> HandlersDictionary = new();

        public static IEnumerable<IDomainEventHandler> GetHandlers(
            Type type, 
            IServiceProvider serviceProvider,
            Assembly assembly)
        {

            var domainEventHandlerTypes = HandlersDictionary.GetOrAdd(
                $"{assembly.GetName().Name}{type.Name}",
                _ =>
                {
                    var domainEventHandlerTypes = assembly
                        .GetTypes()
                        .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler<>).MakeGenericType(type)))
                        .ToArray();

                    return domainEventHandlerTypes;
                });

            List<IDomainEventHandler> handlers = [];

            foreach (var domainEventHandlerType in domainEventHandlerTypes)
            {
                var handler = serviceProvider.GetRequiredService(domainEventHandlerType);
                handlers.Add((domainEventHandlerType as IDomainEventHandler)!);
            }

            return handlers;
        }
    }
}

using EventManagement.Common.Application.EventBuses;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EventManagement.Common.Infrastructure.EventBuses
{
    internal sealed class EventBus(IBus bus, ILogger<EventBus> logger) : IEventBus
    {
        public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) 
            where T : IIntegrationEvent
        {
            logger.LogInformation("Publishing {Event}", integrationEvent.GetType().FullName);
            await bus.Publish(integrationEvent, cancellationToken);
        }
    }
}

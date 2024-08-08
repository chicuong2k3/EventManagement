using EventManagement.Common.Application.EventBuses;
using MassTransit;

namespace EventManagement.Common.Infrastructure.EventBuses
{
    internal sealed class EventBus(IBus bus) : IEventBus
    {
        public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IIntegrationEvent
        {
            await bus.Publish(integrationEvent, cancellationToken);
        }
    }
}

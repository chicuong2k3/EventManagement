namespace EventManagement.Common.Application.EventBuses
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
            where T : IIntegrationEvent;
    }
}

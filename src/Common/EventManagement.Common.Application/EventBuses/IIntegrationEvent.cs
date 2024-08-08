namespace EventManagement.Common.Application.EventBuses
{
    public interface IIntegrationEvent
    {
        Guid Id { get; }
        DateTime OccurredOn { get; }
    }
}

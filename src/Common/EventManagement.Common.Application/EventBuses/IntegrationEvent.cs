namespace EventManagement.Common.Application.EventBuses
{
    public abstract class IntegrationEvent : IIntegrationEvent
    {
        protected IntegrationEvent(Guid id, DateTime occurredOn)
        {
            Id = id;
            OccurredOn = occurredOn;
        }
        protected IntegrationEvent()
        {
            
        }
        public Guid Id { get; init; }

        public DateTime OccurredOn { get; init; }
    }
}

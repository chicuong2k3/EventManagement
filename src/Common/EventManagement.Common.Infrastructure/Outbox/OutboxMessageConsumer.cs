namespace EventManagement.Common.Infrastructure.Outbox
{
    public sealed class OutboxMessageConsumer(
        Guid outboxMessageId, 
        string handlerName)
    {
        public Guid OutboxMessageId { get; init; } = outboxMessageId;
        public string HandlerName { get; init; } = handlerName;
    }
}

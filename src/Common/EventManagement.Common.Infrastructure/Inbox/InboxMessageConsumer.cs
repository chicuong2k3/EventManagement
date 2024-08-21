namespace EventManagement.Common.Infrastructure.Inbox
{
    public sealed class InboxMessageConsumer(
        Guid inboxMessageId,
        string handlerName)
    {
        public Guid InboxMessageId { get; init; } = inboxMessageId;
        public string HandlerName { get; init; } = handlerName;
    }
}

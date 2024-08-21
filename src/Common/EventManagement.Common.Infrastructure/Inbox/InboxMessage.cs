namespace EventManagement.Common.Infrastructure.Inbox
{
    public sealed class InboxMessage
    {
        public Guid Id { get; init; }
        public string Type { get; init; }
        public string Content { get; init; }
        public DateTime OccurredOn { get; init; }
        public DateTime? ProcessedOn { get; init; }
        public string? Error { get; init; }
    }
}

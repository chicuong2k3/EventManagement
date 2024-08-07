namespace EventManagement.Users.Domain.DomainEvents.Users
{
    public sealed class UserRegisteredDomainEvent(Guid userId) : DomainEventBase
    {
        public Guid UserId { get; init; } = userId;
    }
}

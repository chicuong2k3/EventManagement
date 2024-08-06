namespace EventManagement.Users.Domain.DomainEvents.Users
{
    public sealed class UserRegistered(Guid userId) : DomainEventBase
    {
        public Guid UserId { get; init; } = userId;
    }
}

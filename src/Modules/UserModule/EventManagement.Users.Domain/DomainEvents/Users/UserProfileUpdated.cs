namespace EventManagement.Users.Domain.DomainEvents.Users
{
    public sealed class UserProfileUpdated(Guid userId, string firstName, string lastName)
        : DomainEventBase
    {
        public Guid UserId { get; init; } = userId;
        public string FirstName { get; init; } = firstName;
        public string LastName { get; init; } = lastName;
    }
}

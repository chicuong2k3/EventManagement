namespace EventManagement.Users.Domain.Users
{
    public sealed class UserUpdatedDomainEvent(Guid userId, string firstName, string lastName)
        : DomainEvent
    {
        public Guid UserId { get; init; } = userId;
        public string FirstName { get; init; } = firstName;
        public string LastName { get; init; } = lastName;
    }
}

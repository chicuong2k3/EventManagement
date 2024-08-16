using EventManagement.Users.Domain.DomainEvents.Users;

namespace EventManagement.Users.Domain.Entities
{
    public sealed class User : Entity
    {
        private readonly List<Role> roles = [];
        private User()
        {

        }
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string IdentityId { get; private set; }

        public IReadOnlyCollection<Role> Roles => roles.ToList();
        public static User Create(string email, string firstName, string lastName, string identityId)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                IdentityId = identityId
            };

            user.roles.Add(Role.Member);

            user.Raise(new UserRegisteredDomainEvent(user.Id));

            return user;
        }

        public Result Update(string firstName, string lastName)
        {

            if (FirstName != firstName || LastName != lastName)
            {
                FirstName = firstName;
                LastName = lastName;
                Raise(new UserProfileUpdatedDomainEvent(Id, FirstName, LastName));
            }

            return Result.Success();
        }
    }
}

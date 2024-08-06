using EventManagement.Users.Domain.DomainEvents.Users;

namespace EventManagement.Users.Domain.Entities
{
    public sealed class User : Entity
    {
        private User()
        {

        }
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public static User Create(string email, string firstName, string lastName)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            user.Raise(new UserRegistered(user.Id));

            return user;
        }

        public Result Update(string firstName, string lastName)
        {

            if (FirstName != firstName || LastName != lastName)
            {
                FirstName = firstName;
                LastName = lastName;
                Raise(new UserProfileUpdated(Id, FirstName, LastName));
            }

            return Result.Success();
        }
    }
}

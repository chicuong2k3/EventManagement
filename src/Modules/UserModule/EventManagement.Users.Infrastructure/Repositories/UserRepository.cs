

namespace EventManagement.Users.Infrastructure.Repositories
{
    internal sealed class UserRepository(UsersDbContext dbContext) : IUserRepository
    {
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Users.FindAsync(id, cancellationToken);
        }

        public void Insert(User user)
        {
            foreach (var role in user.Roles)
            {
                dbContext.Attach(role);
            }

            dbContext.Users.Add(user);
        }
    }
}

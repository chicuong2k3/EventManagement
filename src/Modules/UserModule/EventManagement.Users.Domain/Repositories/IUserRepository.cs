using EventManagement.Users.Domain.Entities;

namespace EventManagement.Users.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Insert(User user);
    }
}

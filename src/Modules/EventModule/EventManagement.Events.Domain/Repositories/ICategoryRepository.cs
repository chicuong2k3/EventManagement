using EventManagement.Events.Domain.Entities;

namespace EventManagement.Events.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Insert(Category category);
    }
}



namespace EventManagement.Events.Infrastructure.Repositories
{
    internal sealed class CategoryRepository(AppDbContext dbContext) : ICategoryRepository
    {
        public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Categories.FindAsync(id, cancellationToken);
        }

        public void Insert(Category category)
        {
            dbContext.Categories.Add(category);
        }
    }
}

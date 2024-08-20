using EventManagement.Events.Domain.Categories;

namespace EventManagement.Events.Infrastructure.Categories
{
    internal sealed class CategoryRepository(EventsDbContext dbContext) : ICategoryRepository
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

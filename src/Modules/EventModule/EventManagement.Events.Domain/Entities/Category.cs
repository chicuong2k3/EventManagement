
using EventManagement.Events.Domain.DomainEvents.Categories;

namespace EventManagement.Events.Domain.Entities
{
    public sealed class Category : Entity
    {
        private Category()
        {

        }
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool IsArchived { get; private set; }
        public static Category Create(string name)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = name,
                IsArchived = false
            };

            category.Raise(new CategoryCreated(category.Id));

            return category;
        }
        public Result Archive()
        {
            if (IsArchived)
            {
                return Result.Failure(CategoryErrors.AlreadyArchived);
            }

            IsArchived = true;

            Raise(new CategoryArchived(Id));

            return Result.Success();
        }

        public void ChangeName(string name)
        {
            if (Name == name)
            {
                return;
            }

            Name = name;

            Raise(new CategoryNameChanged(Id, Name));
        }
    }
}

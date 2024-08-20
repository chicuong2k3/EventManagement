namespace EventManagement.Events.Domain.Categories
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

            category.Raise(new CategoryCreatedDomainEvent(category.Id));

            return category;
        }
        public Result Archive()
        {
            if (IsArchived)
            {
                return Result.Failure(CategoryErrors.AlreadyArchived);
            }

            IsArchived = true;

            Raise(new CategoryArchivedDomainEvent(Id));

            return Result.Success();
        }

        public Result ChangeName(string name)
        {
            if (Name != name)
            {
                Name = name;

                Raise(new CategoryNameChangedDomainEvent(Id, Name));
            }

            return Result.Success();
        }
    }
}

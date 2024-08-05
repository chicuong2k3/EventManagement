namespace EventManagement.Events.Domain.Errors
{
    public static class CategoryErrors
    {
        public static Error NotFound(Guid categoryId) =>
            Error.NotFound("Category.NotFound", $"The category with the identifier {categoryId} was not found");

        public static readonly Error AlreadyArchived = Error.Problem(
            "Category.AlreadyArchived",
            "The category was already archived");
    }

}

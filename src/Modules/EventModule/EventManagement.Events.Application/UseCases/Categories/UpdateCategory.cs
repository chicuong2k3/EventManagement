


namespace EventManagement.Events.Application.UseCases.Categories;

public sealed record UpdateCategoryCommand(
    Guid Id,
    string Name
) : ICommand;

internal sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

    }
}
internal sealed class UpdateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(command.Id, cancellationToken);

        if (category == null)
        {
            return Result.Failure(CategoryErrors.NotFound(command.Id));
        }

        category.ChangeName(command.Name);

        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }

}

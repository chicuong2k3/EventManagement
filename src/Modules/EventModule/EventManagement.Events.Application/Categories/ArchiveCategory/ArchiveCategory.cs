using EventManagement.Common.Domain;
using EventManagement.Events.Domain.Categories;

namespace EventManagement.Events.Application.Categories.ArchiveCategory;

public sealed record ArchiveCategoryCommand(Guid Id) : ICommand;

internal sealed class ArchiveCategoryCommandValidator : AbstractValidator<ArchiveCategoryCommand>
{
    public ArchiveCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

    }
}
internal sealed class ArchiveCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ArchiveCategoryCommand>
{
    public async Task<Result> Handle(ArchiveCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(command.Id, cancellationToken);

        if (category == null)
        {
            return Result.Failure(CategoryErrors.NotFound(command.Id));
        }

        var result = category.Archive();

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}

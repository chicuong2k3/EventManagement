using EventManagement.Events.Application.Abstractions.Data;

namespace EventManagement.Events.Application.UseCases.Events;

public sealed record CreateEventCommand(
    string Title,
    string Description,
    string Location,
    Guid CategoryId,
    DateTime StartsAt,
    DateTime? EndsAt
) : ICommand<Guid>;

internal sealed class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");

        RuleFor(x => x.StartsAt)
            .NotEmpty().WithMessage("StartsAt is required.");

        RuleFor(x => x.EndsAt)
            .NotEmpty().WithMessage("EndsAt is required.")
            .Must((command, endsAt) => endsAt > command.StartsAt).When(c => c.EndsAt.HasValue)
            .WithMessage("EndsAt must be greater than StartsAt");
    }
}
internal sealed class CreateEventCommandHandler(
    IEventRepository eventRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateEventCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateEventCommand command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

        if (category == null)
        {
            return Result.Failure<Guid>(CategoryErrors.NotFound(command.CategoryId));
        }

        var result = EventEntity.Create(
            command.Title,
            command.Description,
            command.Location,
            command.CategoryId,
            command.StartsAt,
            command.EndsAt
        );

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        eventRepository.Insert(result.Value);
        await unitOfWork.CommitAsync(cancellationToken);

        return result.Value.Id;
    }

}

namespace EventManagement.Events.Application.UseCases.Events;

public sealed record RescheduleEventCommand(
    Guid EventId,
    DateTime StartsAt,
    DateTime? EndsAt
) : ICommand;

internal sealed class RescheduleEventCommandValidator : AbstractValidator<RescheduleEventCommand>
{
    public RescheduleEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("EventId is required.");

        RuleFor(x => x.StartsAt)
            .NotEmpty().WithMessage("StartsAt is required.");

        RuleFor(x => x.EndsAt)
            .NotEmpty().WithMessage("EndsAt is required.")
            .Must((command, endsAt) => endsAt > command.StartsAt).When(c => c.EndsAt.HasValue)
            .WithMessage("EndsAt must be greater than StartsAt");
    }
}
internal sealed class RescheduleEventCommandHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RescheduleEventCommand>
{
    public async Task<Result> Handle(RescheduleEventCommand command, CancellationToken cancellationToken)
    {
        var eventEntity = await eventRepository.GetByIdAsync(command.EventId, cancellationToken);

        if (eventEntity is null)
        {
            return Result.Failure(EventErrors.NotFound(command.EventId));
        }

        var result = eventEntity.Reschedule(command.StartsAt, command.EndsAt);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}

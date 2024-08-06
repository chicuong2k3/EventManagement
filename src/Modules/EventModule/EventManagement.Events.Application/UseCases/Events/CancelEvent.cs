using EventManagement.Events.Application.Abstractions.Data;

namespace EventManagement.Events.Application.UseCases.Events;

public sealed record CancelEventCommand(Guid EventId) : ICommand;

internal sealed class CancelEventCommandValidator : AbstractValidator<CancelEventCommand>
{
    public CancelEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("EventId is required.");
    }
}
internal sealed class CancelEventCommandHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CancelEventCommand>
{
    public async Task<Result> Handle(CancelEventCommand command, CancellationToken cancellationToken)
    {
        var eventEntity = await eventRepository.GetByIdAsync(command.EventId, cancellationToken);

        if (eventEntity == null)
        {
            return Result.Failure(EventErrors.NotFound(command.EventId));
        }

        var result = eventEntity.Cancel();

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}

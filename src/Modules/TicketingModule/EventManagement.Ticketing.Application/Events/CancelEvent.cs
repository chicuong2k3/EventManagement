using EventManagement.Ticketing.Domain.Events;

namespace EventManagement.Ticketing.Application.Events;

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

        eventEntity.Cancel();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

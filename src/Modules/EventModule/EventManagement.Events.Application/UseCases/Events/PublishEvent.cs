using EventManagement.Events.Application.Abstractions.Data;

namespace EventManagement.Events.Application.UseCases.Events;

public sealed record PublishEventCommand(Guid EventId) : ICommand;

internal sealed class PublishEventCommandValidator : AbstractValidator<PublishEventCommand>
{
    public PublishEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("EventId is required.");
    }
}
internal sealed class PublishEventCommandHandler(
    IEventRepository eventRepository,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<PublishEventCommand>
{
    public async Task<Result> Handle(PublishEventCommand command, CancellationToken cancellationToken)
    {
        var eventEntity = await eventRepository.GetByIdAsync(command.EventId, cancellationToken);

        if (eventEntity == null)
        {
            return Result.Failure(EventErrors.NotFound(command.EventId));
        }

        if (!await ticketRepository.ExistsAsync(eventEntity.Id, cancellationToken))
        {
            return Result.Failure(EventErrors.NoTicketsFound);
        }

        var result = eventEntity.Publish();

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }

}

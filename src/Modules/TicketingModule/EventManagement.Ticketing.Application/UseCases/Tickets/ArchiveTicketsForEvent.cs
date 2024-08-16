
namespace EventManagement.Ticketing.Application.UseCases.Tickets;

public sealed record ArchiveTicketsForEventCommand(Guid EventId) : ICommand;
internal sealed class ArchiveTicketsForEventCommandValidator : AbstractValidator<ArchiveTicketsForEventCommand>
{
    public ArchiveTicketsForEventCommandValidator()
    {
        RuleFor(c => c.EventId)
            .NotEmpty().WithMessage("EventId is required.");
    }
}

internal sealed class ArchiveTicketsForEventCommandHandler(
    IEventRepository eventRepository,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ArchiveTicketsForEventCommand>
{
    public async Task<Result> Handle(ArchiveTicketsForEventCommand request, CancellationToken cancellationToken)
    {
        await using (var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken))
        {
            var eventEntity = await eventRepository.GetByIdAsync(request.EventId, cancellationToken);

            if (eventEntity == null)
            {
                return Result.Failure(EventErrors.NotFound(request.EventId));
            }

            var tickets = await ticketRepository.GetForEventAsync(eventEntity, cancellationToken);

            foreach (var ticket in tickets)
            {
                ticket.Archive();
            }

            eventEntity.TicketsArchived();

            await unitOfWork.CommitAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return Result.Success();
        }

            
    }
}

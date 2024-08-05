namespace EventManagement.Events.Application.UseCases.Tickets;

public sealed record CreateTicketCommand(
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    int Quantity
) : ICommand<Guid>;

internal sealed class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("EventId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Price)
            .GreaterThan(decimal.Zero).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}
internal sealed class CreateTicketCommandHandler(
    ITicketRepository ticketRepository,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateTicketCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTicketCommand command, CancellationToken cancellationToken)
    {
        var eventEntity = await eventRepository.GetByIdAsync(command.EventId, cancellationToken);

        if (eventEntity == null)
        {
            return Result.Failure<Guid>(EventErrors.NotFound(command.EventId));
        }

        var ticket = Ticket.Create(
            eventEntity.Id,
            command.Name,
            command.Price,
            command.Currency,
            command.Quantity);

        ticketRepository.Insert(ticket);
        await unitOfWork.CommitAsync(cancellationToken);
        return ticket.Id;
    }

}

namespace EventManagement.Events.Application.UseCases.Tickets;

public sealed record UpdateTicketPriceCommand(
    Guid Id,
    decimal Price
) : ICommand;

internal sealed class UpdateTicketPriceCommandValidator : AbstractValidator<UpdateTicketPriceCommand>
{
    public UpdateTicketPriceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Price)
            .GreaterThan(decimal.Zero).WithMessage("Price must be greater than 0.");

    }
}
internal sealed class UpdateTicketPriceCommandHandler(
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateTicketPriceCommand>
{
    public async Task<Result> Handle(UpdateTicketPriceCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(command.Id, cancellationToken);

        if (ticket == null)
        {
            return Result.Failure(TicketErrors.NotFound(command.Id));
        }

        ticket.UpdatePrice(command.Price);

        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }

}

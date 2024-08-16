namespace EventManagement.Ticketing.Application.UseCases.TicketTypes;

public sealed record UpdateTicketTypePriceCommand(
    Guid Id,
    decimal Price
) : ICommand;

internal sealed class UpdateTicketTypePriceCommandValidator : AbstractValidator<UpdateTicketTypePriceCommand>
{
    public UpdateTicketTypePriceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Price)
            .GreaterThan(decimal.Zero).WithMessage("Price must be greater than 0.");

    }
}
internal sealed class UpdateTicketTypePriceCommandHandler(
    ITicketTypeRepository ticketTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateTicketTypePriceCommand>
{
    public async Task<Result> Handle(UpdateTicketTypePriceCommand command, CancellationToken cancellationToken)
    {
        var ticketType = await ticketTypeRepository.GetByIdAsync(command.Id, cancellationToken);

        if (ticketType == null)
        {
            return Result.Failure(TicketTypeErrors.NotFound(command.Id));
        }

        ticketType.UpdatePrice(command.Price);

        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }

}

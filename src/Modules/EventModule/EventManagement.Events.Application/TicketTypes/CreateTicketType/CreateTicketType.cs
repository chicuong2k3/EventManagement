using EventManagement.Common.Domain;
using EventManagement.Events.Domain.TicketTypes;

namespace EventManagement.Events.Application.TicketTypes.CreateTicketType;

public sealed record CreateTicketTypeCommand(
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    int Quantity
) : ICommand<Guid>;

internal sealed class CreateTicketTypeCommandValidator : AbstractValidator<CreateTicketTypeCommand>
{
    public CreateTicketTypeCommandValidator()
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
internal sealed class CreateTicketTypeCommandHandler(
    ITicketTypeRepository ticketTypeRepository,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateTicketTypeCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTicketTypeCommand command, CancellationToken cancellationToken)
    {
        var eventEntity = await eventRepository.GetByIdAsync(command.EventId, cancellationToken);

        if (eventEntity == null)
        {
            return Result.Failure<Guid>(EventErrors.NotFound(command.EventId));
        }

        var ticketType = TicketType.Create(
            eventEntity.Id,
            command.Name,
            command.Price,
            command.Currency,
            command.Quantity);

        ticketTypeRepository.Insert(ticketType);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ticketType.Id;
    }

}

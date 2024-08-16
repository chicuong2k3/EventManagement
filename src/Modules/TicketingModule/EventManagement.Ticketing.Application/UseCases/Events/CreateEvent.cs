namespace EventManagement.Ticketing.Application.UseCases.Events;
public sealed record TicketTypeRequest(
        Guid TicketTypeId,
        Guid EventId,
        string Name,
        decimal Price,
        string Currency,
        int Quantity
        );
public sealed record CreateEventCommand(
    Guid Id,
    string Title,
    string Description,
    string Location,
    DateTime StartsAt,
    DateTime? EndsAt,
    List<TicketTypeRequest> TicketTypes
) : ICommand;

internal sealed class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.");

        RuleFor(x => x.StartsAt)
            .NotEmpty().WithMessage("StartsAt is required.");

        RuleFor(x => x.EndsAt)
            .NotEmpty().WithMessage("EndsAt is required.")
            .Must((command, endsAt) => endsAt > command.StartsAt).When(c => c.EndsAt.HasValue)
            .WithMessage("EndsAt must be greater than StartsAt");

        RuleForEach(x => x.TicketTypes)
            .ChildRules(t =>
            {
                t.RuleFor(r => r.EventId)
                    .NotEmpty().WithMessage("EventId is required.");

                t.RuleFor(r => r.Name)
                    .NotEmpty().WithMessage("Name is required.");

                t.RuleFor(r => r.Price)
                    .GreaterThan(decimal.Zero).WithMessage("Price must be greater than 0.");

                t.RuleFor(r => r.Currency)
                    .NotEmpty().WithMessage("Currency is required.");

                t.RuleFor(r => r.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
            });
    }
}
internal sealed class CreateEventCommandHandler(
    IEventRepository eventRepository,
    ITicketTypeRepository ticketTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateEventCommand>
{
    public async Task<Result> Handle(CreateEventCommand command, CancellationToken cancellationToken)
    {
        
        var eventEntity = EventEntity.Create(
            command.Id,
            command.Title,
            command.Description,
            command.Location,
            command.StartsAt,
            command.EndsAt
        );

        eventRepository.Insert(eventEntity);


        var ticketTypes = command.TicketTypes
                                .Select(x => TicketType.Create(
                                    x.TicketTypeId, 
                                    x.EventId, 
                                    x.Name, 
                                    x.Price, 
                                    x.Currency, 
                                    x.Quantity)
                                );

        ticketTypeRepository.InsertRange(ticketTypes);


        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }

}

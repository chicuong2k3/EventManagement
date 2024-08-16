using EventManagement.Events.PublicApi;

namespace EventManagement.Ticketing.Application.UseCases.Carts.CommandHandlers;

public sealed record AddItemToCartCommand(
    Guid CustomerId,
    Guid TicketTypeId,
    int Quantity
) : ICommand;

internal sealed class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");

        RuleFor(x => x.TicketTypeId)
            .NotEmpty().WithMessage("TicketTypeId is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}
internal sealed class AddItemToCartCommandHandler(
    CartService cartService,
    ICustomerRepository customerRepository,
    IEventsApi ticketsApi)
    : ICommandHandler<AddItemToCartCommand>
{
    public async Task<Result> Handle(AddItemToCartCommand command, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);
        if (customer == null)
        {
            return Result.Failure(CustomerErrors.NotFound(command.CustomerId));
        }

        var ticketType = await ticketsApi.GetTicketTypeAsync(command.TicketTypeId, cancellationToken);
        if (ticketType == null)
        {
            return Result.Failure(TicketTypeErrors.NotFound(command.TicketTypeId));
        }


        var cartItem = new CartItem()
        {
            TicketTypeId = ticketType.Id,
            Price = ticketType.Price,
            Currency = ticketType.Currency,
            Quantity = command.Quantity
        };

        await cartService.AddItemAsync(customer.Id, cartItem, cancellationToken);

        return Result.Success();
    }
}


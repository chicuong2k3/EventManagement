using EventManagement.Ticketing.Application.Carts;
using EventManagement.Ticketing.Domain.Orders;
using EventManagement.Ticketing.Domain.Payments;
using EventManagement.Ticketing.Domain.TicketTypes;

namespace EventManagement.Ticketing.Application.Orders.CreateOrder;

public sealed record CreateOrderCommand(Guid CustomerId) : ICommand;

internal sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");

    }
}
internal sealed class CreateOrderCommandHandler(
    ICustomerRepository customerRepository,
    IOrderRepository orderRepository,
    ITicketTypeRepository ticketTypeRepository,
    IPaymentRepository paymentRepository,
    IPaymentService paymentService,
    CartService cartService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateOrderCommand>
{
    public async Task<Result> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        await using (var transaction = await unitOfWork.BeginTransactionAsync())
        {
            var customer = await customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);
            if (customer == null)
            {
                return Result.Failure(CustomerErrors.NotFound(command.CustomerId));
            }

            var order = Order.Create(customer);

            var cart = await cartService.GetCartAsync(customer.Id, cancellationToken);

            if (!cart.CartItems.Any())
            {
                return Result.Failure(CartErrors.Empty);
            }

            foreach (var cartItem in cart.CartItems)
            {
                var ticketType = await ticketTypeRepository.GetByIdWithLockAsync(
                    cartItem.TicketTypeId,
                    cancellationToken);

                if (ticketType == null)
                {
                    return Result.Failure(TicketTypeErrors.NotFound(cartItem.TicketTypeId));
                }

                var result = ticketType.UpdateQuantity(cartItem.Quantity);

                if (result.IsFailure)
                {
                    return Result.Failure(result.Error);
                }

                order.AddItem(ticketType, cartItem.Quantity, cartItem.Price, ticketType.Currency);
            }

            orderRepository.Insert(order);

            // We're faking a payment gateway request here...
            var paymentResponse = await paymentService.ChargeAsync(order.TotalPrice, order.Currency);

            var payment = Payment.Create(
                order,
                paymentResponse.TransactionId,
                paymentResponse.Amount,
                paymentResponse.Currency);

            paymentRepository.Insert(payment);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await cartService.ClearCartAsync(customer.Id, cancellationToken);
        }

        return Result.Success();
    }
}


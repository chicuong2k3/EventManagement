using EventManagement.Ticketing.Domain.Orders;
using EventManagement.Ticketing.Domain.Tickets;
using EventManagement.Ticketing.Domain.TicketTypes;

namespace EventManagement.Ticketing.Application.Tickets;

public sealed record CreateTicketBatchCommand(Guid OrderId) : ICommand;
internal sealed class CreateTicketBatchCommandValidator : AbstractValidator<CreateTicketBatchCommand>
{
    public CreateTicketBatchCommandValidator()
    {
        RuleFor(c => c.OrderId)
            .NotEmpty().WithMessage("OrderId is required.");
    }
}
internal sealed class CreateTicketBatch(
    IOrderRepository orderRepository,
    ITicketTypeRepository ticketTypeRepository,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateTicketBatchCommand>
{
    public async Task<Result> Handle(CreateTicketBatchCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order == null)
        {
            return Result.Failure(OrderErrors.NotFound(request.OrderId));
        }

        var result = order.IssueTickets();

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        List<Ticket> tickets = [];
        foreach (var orderItem in order.OrderItems)
        {
            var ticketType = await ticketTypeRepository.GetByIdAsync(orderItem.TicketTypeId, cancellationToken);

            if (ticketType == null)
            {
                return Result.Failure(TicketTypeErrors.NotFound(orderItem.TicketTypeId));
            }

            for (int i = 0; i < orderItem.Quantity; i++)
            {
                var ticket = Ticket.Create(order, ticketType);

                tickets.Add(ticket);
            }
        }

        ticketRepository.InsertRange(tickets);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

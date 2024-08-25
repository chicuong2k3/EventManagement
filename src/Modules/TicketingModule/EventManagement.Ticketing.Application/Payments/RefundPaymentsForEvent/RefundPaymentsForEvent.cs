using EventManagement.Ticketing.Domain.Events;
using EventManagement.Ticketing.Domain.Payments;

namespace EventManagement.Ticketing.Application.Payments.RefundPaymentsForEvent;

public sealed record RefundPaymentsForEventCommand(Guid EventId) : ICommand;

internal sealed class RefundPaymentsForEventCommandValidator : AbstractValidator<RefundPaymentsForEventCommand>
{
    public RefundPaymentsForEventCommandValidator()
    {
        RuleFor(c => c.EventId).NotEmpty();
    }
}

internal sealed class RefundPaymentsForEventCommandHandler(
    IEventRepository eventRepository,
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RefundPaymentsForEventCommand>
{
    public async Task<Result> Handle(RefundPaymentsForEventCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        var eventEntity = await eventRepository.GetByIdAsync(request.EventId, cancellationToken);

        if (eventEntity is null)
        {
            return Result.Failure(EventErrors.NotFound(request.EventId));
        }

        var payments = await paymentRepository.GetForEventAsync(eventEntity, cancellationToken);

        foreach (var payment in payments)
        {
            payment.Refund(payment.Amount - (payment.AmountRefunded ?? decimal.Zero));
        }

        eventEntity.PaymentsRefunded();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}

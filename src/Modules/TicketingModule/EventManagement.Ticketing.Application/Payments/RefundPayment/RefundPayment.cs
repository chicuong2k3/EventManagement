using EventManagement.Ticketing.Domain.Payments;

namespace EventManagement.Ticketing.Application.Payments.RefundPayment;

public sealed record RefundPaymentCommand(Guid PaymentId, decimal Amount) : ICommand;

internal sealed class RefundPaymentCommandValidator : AbstractValidator<RefundPaymentCommand>
{
    public RefundPaymentCommandValidator()
    {
        RuleFor(c => c.PaymentId)
            .NotEmpty().WithMessage("PaymentId is required.");

        RuleFor(c => c.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");
    }
}
internal sealed class RefundPaymentCommandHandler(
    IPaymentRepository paymentRepository, 
    IUnitOfWork unitOfWork) : ICommandHandler<RefundPaymentCommand>
{
    public async Task<Result> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetByIdAsync(request.PaymentId, cancellationToken);

        if (payment == null)
        {
            return Result.Failure(PaymentErrors.NotFound(request.PaymentId));
        }

        var result = payment.Refund(request.Amount);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
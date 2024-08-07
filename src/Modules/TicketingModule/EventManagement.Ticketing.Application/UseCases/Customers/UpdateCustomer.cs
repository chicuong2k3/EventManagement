

namespace EventManagement.Ticketing.Application.UseCases.Customers;

public sealed record UpdateCustomerCommand(
    Guid CustomerId,
    string FirstName,
    string LastName
) : ICommand;

internal sealed class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required.");

    }
}
internal sealed class UpdateCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateCustomerCommand>
{
    public async Task<Result> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);

        if (customer == null)
        {
            return Result.Failure(CustomerErrors.NotFound(command.CustomerId));
        }

        customer.Update(command.FirstName, command.LastName);

        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}

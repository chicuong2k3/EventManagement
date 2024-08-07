
using EventManagement.Ticketing.Domain.Entities;

namespace EventManagement.Ticketing.Application.UseCases.Customers;

public sealed record CreateCustomerCommand(
    Guid CustomerId,
    string Email,
    string FirstName,
    string LastName
) : ICommand;

internal sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required.");

    }
}
internal sealed class CreateCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCustomerCommand>
{
    public async Task<Result> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {

        var customer = Customer.Create(command.CustomerId, command.Email, command.FirstName, command.LastName);
            
        customerRepository.Insert(customer);

        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}

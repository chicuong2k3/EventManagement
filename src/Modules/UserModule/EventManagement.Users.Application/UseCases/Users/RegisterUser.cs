namespace EventManagement.Users.Application.UseCases.Users;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName
) : ICommand<Guid>;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Name is required.")
            .EmailAddress();

        RuleFor(x => x.Password)
            .MinimumLength(6).WithMessage("Password must have at least 6 characters.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required.");

    }
}
internal sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        // Validate duplicate email

        var user = User.Create(command.Email, command.FirstName, command.LastName);

        userRepository.Insert(user);

        await unitOfWork.CommitAsync(cancellationToken);

        return user.Id;
    }

}

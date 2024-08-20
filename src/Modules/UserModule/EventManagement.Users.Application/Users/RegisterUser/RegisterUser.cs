using EventManagement.Users.Application.Abstractions.Data;
using EventManagement.Users.Application.Abstractions.Identity;
using Microsoft.Extensions.Logging;

namespace EventManagement.Users.Application.Users.RegisterUser;

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
    IUnitOfWork unitOfWork,
    IIdentityProviderService identityProviderService)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await identityProviderService.RegisterUserAsync(
            command.Email,
            command.Password,
            command.FirstName,
            command.LastName,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        var user = User.Create(command.Email, command.FirstName, command.LastName, result.Value);
        userRepository.Insert(user);


        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }

}

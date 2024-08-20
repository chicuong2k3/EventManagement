using EventManagement.Users.Application.Abstractions.Data;

namespace EventManagement.Users.Application.Users.UpdateUser;

public sealed record UpdateUserProfileCommand(
    Guid Id,
    string FirstName,
    string LastName
) : ICommand;

internal sealed class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required.");

    }
}
internal sealed class UpdateUserProfileCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateUserProfileCommand>
{
    public async Task<Result> Handle(UpdateUserProfileCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(command.Id, cancellationToken);

        if (user == null)
        {
            return Result.Failure(UserErrors.NotFound(command.Id));
        }

        user.Update(command.FirstName, command.LastName);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}

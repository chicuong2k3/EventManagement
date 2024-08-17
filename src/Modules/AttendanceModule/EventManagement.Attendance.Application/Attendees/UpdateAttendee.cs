namespace EventManagement.Attendance.Application.Attendees;

public sealed record UpdateAttendeeCommand(Guid AttendeeId, string FirstName, string LastName) : ICommand;

internal sealed class UpdateAttendeeCommandValidator : AbstractValidator<UpdateAttendeeCommand>
{
    public UpdateAttendeeCommandValidator()
    {
        RuleFor(c => c.AttendeeId).NotEmpty();
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
    }
}

internal sealed class UpdateAttendee(IAttendeeRepository attendeeRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateAttendeeCommand>
{
    public async Task<Result> Handle(UpdateAttendeeCommand request, CancellationToken cancellationToken)
    {
        var attendee = await attendeeRepository.GetAsync(request.AttendeeId, cancellationToken);

        if (attendee is null)
        {
            return Result.Failure(AttendeeErrors.NotFound(request.AttendeeId));
        }

        attendee.Update(request.FirstName, request.LastName);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

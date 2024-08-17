

namespace EventManagement.Attendance.Application.Attendees;

public sealed record CheckInAttendeeCommand(Guid AttendeeId, Guid TicketId) : ICommand;

internal sealed class CheckInAttendeeCommandValidator : AbstractValidator<CheckInAttendeeCommand>
{
    public CheckInAttendeeCommandValidator()
    {
        RuleFor(c => c.TicketId).NotEmpty();
    }
}

internal sealed class CheckInAttendee(
    IAttendeeRepository attendeeRepository,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork,
    ILogger<CheckInAttendee> logger)
    : ICommandHandler<CheckInAttendeeCommand>
{
    public async Task<Result> Handle(CheckInAttendeeCommand request, CancellationToken cancellationToken)
    {
        var attendee = await attendeeRepository.GetAsync(request.AttendeeId, cancellationToken);

        if (attendee is null)
        {
            return Result.Failure(AttendeeErrors.NotFound(request.AttendeeId));
        }

        var ticket = await ticketRepository.GetAsync(request.TicketId, cancellationToken);

        if (ticket is null)
        {
            return Result.Failure(TicketErrors.NotFound);
        }

        Result result = attendee.CheckIn(ticket);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (result.IsFailure)
        {
            logger.LogWarning(
                "Check in failed: {AttendeeId}, {TicketId}, {@Error}",
                attendee.Id,
                ticket.Id,
                result.Error);
        }

        return result;
    }
}

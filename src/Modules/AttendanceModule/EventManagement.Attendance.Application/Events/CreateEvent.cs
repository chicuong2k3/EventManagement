using EventManagement.Attendance.Application.Abstractions.Data;
using EventManagement.Attendance.Domain.Events;

namespace EventManagement.Attendance.Application.Events;

public sealed record CreateEventCommand(
    Guid EventId,
    string Title,
    string Description,
    string Location,
    DateTime StartsAt,
    DateTime? EndsAt) : ICommand;

internal sealed class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(c => c.EventId).NotEmpty();
        RuleFor(c => c.Title).NotEmpty();
        RuleFor(c => c.Description).NotEmpty();
        RuleFor(c => c.Location).NotEmpty();
        RuleFor(c => c.StartsAt).NotEmpty();
        RuleFor(c => c.EndsAt).Must((cmd, endsAt) => endsAt > cmd.StartsAt).When(c => c.EndsAt.HasValue);
    }
}

internal sealed class CreateEvent(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateEventCommand>
{
    public async Task<Result> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = Event.Create(
            request.EventId,
            request.Title,
            request.Description,
            request.Location,
            request.StartsAt,
            request.EndsAt);

        eventRepository.Insert(@event);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

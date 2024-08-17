using EventManagement.Common.Domain;

namespace EventManagement.Attendance.Domain.Events;

public sealed class Event : Entity
{
    private Event()
    {
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public string Location { get; private set; }

    public DateTime StartsAt { get; private set; }

    public DateTime? EndsAt { get; private set; }

    public static Event Create(
        Guid id,
        string title,
        string description,
        string location,
        DateTime startsAt,
        DateTime? endsAt)
    {
        var @event = new Event
        {
            Id = id,
            Title = title,
            Description = description,
            Location = location,
            StartsAt = startsAt,
            EndsAt = endsAt
        };

        @event.Raise(new EventCreatedDomainEvent(
            @event.Id,
            @event.Title,
            @event.Description,
            @event.Location,
            @event.StartsAt,
            @event.EndsAt));

        return @event;
    }
}

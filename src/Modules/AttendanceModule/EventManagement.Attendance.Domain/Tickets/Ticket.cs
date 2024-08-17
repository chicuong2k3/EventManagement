using EventManagement.Attendance.Domain.Attendees;
using EventManagement.Attendance.Domain.Events;
using EventManagement.Common.Domain;

namespace EventManagement.Attendance.Domain.Tickets;

public sealed class Ticket : Entity
{
    private Ticket()
    {
    }

    public Guid Id { get; private set; }

    public Guid AttendeeId { get; private set; }

    public Guid EventId { get; private set; }

    public string Code { get; private set; }

    public DateTime? UsedAt { get; private set; }

    public static Ticket Create(Guid ticketId, Attendee attendee, Event @event, string code)
    {
        var ticket = new Ticket
        {
            Id = ticketId,
            AttendeeId = attendee.Id,
            EventId = @event.Id,
            Code = code
        };

        ticket.Raise(new TicketCreatedDomainEvent(ticket.Id, ticket.EventId));

        return ticket;
    }

    internal void MarkAsUsed()
    {
        UsedAt = DateTime.UtcNow;

        Raise(new TicketUsedDomainEvent(Id));
    }
}

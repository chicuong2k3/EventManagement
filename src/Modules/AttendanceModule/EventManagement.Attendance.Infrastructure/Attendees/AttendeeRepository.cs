using EventManagement.Attendance.Infrastructure.Database;

namespace EventManagement.Attendance.Infrastructure.Attendees
{
    internal sealed class AttendeeRepository(AttendanceDbContext context) : IAttendeeRepository
    {
        public async Task<Attendee?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await context.Attendees.FindAsync(id, cancellationToken);
        }

        public void Insert(Attendee attendee)
        {
            context.Attendees.Add(attendee);
        }
    }
}

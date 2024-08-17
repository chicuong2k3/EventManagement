
using EventManagement.Common.Infrastructure.Authentication;

namespace EventManagement.Attendance.Infrastructure.Authentication;

internal sealed class AttendanceContext(IHttpContextAccessor httpContextAccessor) : IAttendanceContext
{
    public Guid AttendeeId => httpContextAccessor.HttpContext?.User.GetUserId() ??
                              throw new InternalServerException("User identifier is unavailable");
}

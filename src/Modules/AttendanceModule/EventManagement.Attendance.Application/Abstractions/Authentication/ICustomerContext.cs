﻿namespace EventManagement.Attendance.Application.Abstractions.Authentication;

public interface IAttendanceContext
{
    Guid AttendeeId { get; }
}

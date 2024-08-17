namespace EventManagement.ArchitectureTests.Abstractions
{
    public abstract class TestBase
    {
        protected const string UsersNamespace = "EventManagement.Users";
        protected const string UsersIntegrationEventsNamespace = "EventManagement.Users.IntegrationEvents";


        protected const string EventsNamespace = "EventManagement.Events";
        protected const string EventsIntegrationEventsNamespace = "EventManagement.Events.IntegrationEvents";


        protected const string TicketingNamespace = "EventManagement.Ticketing";
        protected const string TicketingIntegrationEventsNamespace = "EventManagement.Ticketing.IntegrationEvents";


        protected const string AttendanceNamespace = "EventManagement.Attendance";
        protected const string AttendanceIntegrationEventsNamespace = "EventManagement.Attendance.IntegrationEvents";
    }
}

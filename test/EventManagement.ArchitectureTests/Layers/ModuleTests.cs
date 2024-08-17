using EventManagement.ArchitectureTests.Abstractions;
using NetArchTest.Rules;
using System.Reflection;

namespace EventManagement.ArchitectureTests.Layers
{
    
    public class ModuleTests : TestBase
    {
        [Fact]
        public void UsersModule_ShouldNotHaveDependencyOnAnyOtherModule()
        {
            string[] otherModules = [EventsNamespace, TicketingNamespace, AttendanceNamespace];
            string[] integrationEventsModules = [
                EventsIntegrationEventsNamespace, 
                TicketingIntegrationEventsNamespace,
                AttendanceIntegrationEventsNamespace
            ];

            List<Assembly> usersAssemblies = [
                typeof(Users.Domain.Entities.User).Assembly,
                Users.Application.AssemblyReference.Assembly,
                typeof(Users.Infrastructure.DependencyInjection).Assembly
            ];


            Types.InAssemblies(usersAssemblies)
                .That().DoNotHaveDependencyOnAny(integrationEventsModules)
                .Should().NotHaveDependencyOnAny(otherModules)
                .GetResult()
                .ShouldBeSuccessful();
        }
        [Fact]
        public void TicketingModule_ShouldNotHaveDependencyOnAnyOtherModule()
        {
            string[] otherModules = [EventsNamespace, UsersNamespace, AttendanceNamespace];
            string[] integrationEventsModules = [
                EventsIntegrationEventsNamespace,
                UsersIntegrationEventsNamespace,
                AttendanceIntegrationEventsNamespace
            ];

            List<Assembly> ticketingAssemblies = [
                typeof(Ticketing.Domain.Entities.Ticket).Assembly,
                Ticketing.Application.AssemblyReference.Assembly,
                typeof(Ticketing.Infrastructure.DependencyInjection).Assembly
            ];


            Types.InAssemblies(ticketingAssemblies)
                .That().DoNotHaveDependencyOnAny(integrationEventsModules)
                .Should().NotHaveDependencyOnAny(otherModules)
                .GetResult()
                .ShouldBeSuccessful();
        }
        [Fact]
        public void EventsModule_ShouldNotHaveDependencyOnAnyOtherModule()
        {
            string[] otherModules = [TicketingNamespace, UsersNamespace, AttendanceNamespace];
            string[] integrationEventsModules = [
                TicketingIntegrationEventsNamespace,
                UsersIntegrationEventsNamespace,
                AttendanceIntegrationEventsNamespace
            ];

            List<Assembly> eventsAssemblies = [
                typeof(Events.Domain.Entities.EventEntity).Assembly,
                Events.Application.AssemblyReference.Assembly,
                typeof(Events.Infrastructure.DependencyInjection).Assembly
            ];


            Types.InAssemblies(eventsAssemblies)
                .That().DoNotHaveDependencyOnAny(integrationEventsModules)
                .Should().NotHaveDependencyOnAny(otherModules)
                .GetResult()
                .ShouldBeSuccessful();
        }

        [Fact]
        public void AttendanceModule_ShouldNotHaveDependencyOnAnyOtherModule()
        {
            string[] otherModules = [TicketingNamespace, UsersNamespace, EventsNamespace];
            string[] integrationEventsModules = [
                TicketingIntegrationEventsNamespace,
                UsersIntegrationEventsNamespace,
                EventsIntegrationEventsNamespace
            ];

            List<Assembly> attendanceAssemblies = [
                typeof(Attendance.Domain.Attendees.Attendee).Assembly,
                Attendance.Application.AssemblyReference.Assembly,
                typeof(Attendance.Infrastructure.DependencyInjection).Assembly
            ];


            Types.InAssemblies(attendanceAssemblies)
                .That().DoNotHaveDependencyOnAny(integrationEventsModules)
                .Should().NotHaveDependencyOnAny(otherModules)
                .GetResult()
                .ShouldBeSuccessful();
        }
    }
}

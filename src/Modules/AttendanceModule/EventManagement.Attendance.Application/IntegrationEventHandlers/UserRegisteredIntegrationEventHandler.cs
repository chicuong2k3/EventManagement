using Evently.Modules.Attendance.Application.Attendees.CreateAttendee;
using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Exceptions;
using EventManagement.Users.IntegrationEvents;
using MediatR;

namespace EventManagement.Attendance.Application.IntegrationEventHandlers
{
    internal sealed class UserRegisteredIntegrationEventHandler(ISender sender)
        : IntegrationEventHandler<UserRegisteredIntegrationEvent>
    {
        public override async Task Handle(UserRegisteredIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var result = await sender.Send(new CreateAttendeeCommand(
                integrationEvent.UserId,
                integrationEvent.Email,
                integrationEvent.FirstName,
                integrationEvent.LastName), cancellationToken);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(CreateAttendeeCommand), result.Error);
            }
        }
    }
}

using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Exceptions;
using EventManagement.Ticketing.Application.Customers;
using EventManagement.Users.IntegrationEvents;
using MediatR;

namespace EventManagement.Ticketing.Application.IntegrationEventHandlers
{
    public class UserUpdatedIntegrationEventHandler(ISender sender)
        : IntegrationEventHandler<UserUpdatedIntegrationEvent>
    {
        public override async Task Handle(
            UserUpdatedIntegrationEvent integrationEvent,
            CancellationToken cancellationToken = default)
        {
            var command = new UpdateCustomerCommand(
                integrationEvent.UserId,
                integrationEvent.FirstName,
                integrationEvent.LastName);

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(UpdateCustomerCommand), result.Error);
            }
        }
    }
}

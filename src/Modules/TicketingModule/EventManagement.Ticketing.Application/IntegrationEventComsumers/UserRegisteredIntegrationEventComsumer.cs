using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Exceptions;
using EventManagement.Ticketing.Application.Customers;
using EventManagement.Users.IntegrationEvents;
using MediatR;

namespace EventManagement.Ticketing.Application.IntegrationEventComsumers
{
    public class UserRegisteredIntegrationEventComsumer(ISender sender)
        : IntegrationEventHandler<UserRegisteredIntegrationEvent>
    {
        public override async Task Handle(
            UserRegisteredIntegrationEvent integrationEvent,
            CancellationToken cancellationToken = default)
        {
            var command = new CreateCustomerCommand(
                integrationEvent.UserId,
                integrationEvent.Email,
                integrationEvent.FirstName,
                integrationEvent.LastName);

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(CreateCustomerCommand), result.Error);
            }
        }
    }
}



using EventManagement.Common.Application.Exceptions;
using EventManagement.Ticketing.Application.Customers;
using EventManagement.Users.IntegrationEvents;
using MediatR;

namespace EventManagement.Ticketing.Application.IntegrationEventComsumers
{
    public class UserRegisteredIntegrationEventComsumer(ISender sender)
        : IConsumer<UserRegisteredIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
        {
            var command = new CreateCustomerCommand(
                context.Message.UserId,
                context.Message.Email,
                context.Message.FirstName,
                context.Message.LastName);

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(CreateCustomerCommand), result.Error);
            }
        }
    }
}

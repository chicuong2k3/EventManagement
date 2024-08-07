

using EventManagement.Common.Application.Exceptions;
using EventManagement.Users.Application.UseCases.Users;

namespace EventManagement.Users.Application.DomainEventHandlers
{
    internal sealed class UserRegisteredDomainEventHandler(
        ITicketingApi ticketingApi,
        ISender sender)
        : IDomainEventHandler<UserRegisteredDomainEvent>
    {
        public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetUserByIdQuery(notification.Id));

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(GetUserByIdQuery), result.Error);
            }

            await ticketingApi.CreateCustomerAsync(
                result.Value.Id,
                result.Value.Email,
                result.Value.FirstName,
                result.Value.LastName,
                cancellationToken
            );
        }
    }
}

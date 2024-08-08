


using EventManagement.Users.IntegrationEvents;

namespace EventManagement.Users.Application.DomainEventHandlers
{
    internal sealed class UserRegisteredDomainEventHandler(
        IEventBus eventBus,
        ISender sender)
        : IDomainEventHandler<UserRegisteredDomainEvent>
    {
        public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
                var result = await sender.Send(new GetUserByIdQuery(notification.UserId), cancellationToken);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(GetUserByIdQuery), result.Error);
            }

            await eventBus.PublishAsync(
                new UserRegisteredIntegrationEvent(
                    notification.Id,
                    notification.OccurredOn,
                    result.Value.Id,
                    result.Value.Email,
                    result.Value.FirstName,
                    result.Value.LastName
                ), cancellationToken);
        }
    }
}

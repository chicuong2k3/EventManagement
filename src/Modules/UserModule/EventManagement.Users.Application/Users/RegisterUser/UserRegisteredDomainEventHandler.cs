
namespace EventManagement.Users.Application.Users.RegisterUser
{
    internal sealed class UserRegisteredDomainEventHandler(
        IEventBus eventBus,
        ISender sender)
        : DomainEventHandler<UserRegisteredDomainEvent>
    {
        public override async Task Handle(
            UserRegisteredDomainEvent domainEvent, 
            CancellationToken cancellationToken = default)
        {
            var result = await sender.Send(new GetUserByIdQuery(domainEvent.UserId), cancellationToken);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(GetUserByIdQuery), result.Error);
            }

            await eventBus.PublishAsync(
                new UserRegisteredIntegrationEvent(
                    domainEvent.Id,
                    domainEvent.OccurredOn,
                    result.Value.Id,
                    result.Value.Email,
                    result.Value.FirstName,
                    result.Value.LastName
                ), cancellationToken);
        }

    }
}

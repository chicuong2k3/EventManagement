namespace EventManagement.Users.Application.Users.UpdateUser
{
    internal sealed class UserUpdatedDomainEventHandler(
        IEventBus eventBus,
        ISender sender)
        : DomainEventHandler<UserUpdatedDomainEvent>
    {
        public override async Task Handle(
            UserUpdatedDomainEvent domainEvent, 
            CancellationToken cancellationToken = default)
        {
            var result = await sender.Send(new GetUserByIdQuery(domainEvent.UserId), cancellationToken);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(GetUserByIdQuery), result.Error);
            }

            await eventBus.PublishAsync(
                new UserUpdatedIntegrationEvent(
                    domainEvent.Id,
                    domainEvent.OccurredOn,
                    result.Value.Id,
                    result.Value.FirstName,
                    result.Value.LastName
                ), cancellationToken);
        }
    }
}

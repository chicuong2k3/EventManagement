namespace EventManagement.Events.Application.Events.PublishEvent
{
    internal class EventPublishedDomainEventHandler(
        IEventBus eventBus,
        ISender sender) : DomainEventHandler<EventPublishedDomainEvent>
    {
        public override async Task Handle(EventPublishedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetEventByIdQuery(domainEvent.EventId), cancellationToken);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(GetEventByIdQuery), result.Error);
            }

            await eventBus.PublishAsync(new EventPublishedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOn,
                result.Value.Id,
                result.Value.Title,
                result.Value.Description,
                result.Value.Location,
                result.Value.StartsAt,
                result.Value.EndsAt,
                result.Value.TicketTypes
                .Select(x => new TicketTypeModel()
                {
                    Id = x.TicketTypeId,
                    EventId = result.Value.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Currency = x.Currency
                })
                .ToList()
            ));
        }
    }
}

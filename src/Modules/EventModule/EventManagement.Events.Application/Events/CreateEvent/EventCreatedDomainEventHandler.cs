using EventManagement.Events.Application.TicketTypes;

namespace EventManagement.Events.Application.Events.CreateEvent
{
    internal class EventCreatedDomainEventHandler(
        IEventBus eventBus,
        ISender sender) : DomainEventHandler<EventCreatedDomainEvent>
    {
        public override async Task Handle(EventCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetEventByIdQuery(domainEvent.EventEntityId), cancellationToken);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(GetTicketTypeByIdQuery), result.Error);
            }

            await eventBus.PublishAsync(
                new EventCreatedIntegrationEvent(
                    domainEvent.Id,
                    domainEvent.OccurredOn,
                    result.Value.Id,
                    result.Value.Title,
                    result.Value.Description,
                    result.Value.Location,
                    result.Value.StartsAt,
                    result.Value.EndsAt
                ), cancellationToken);
        }
    }
}

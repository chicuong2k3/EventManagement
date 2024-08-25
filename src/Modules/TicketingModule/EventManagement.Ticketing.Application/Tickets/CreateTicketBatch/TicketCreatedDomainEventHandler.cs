using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Exceptions;
using EventManagement.Ticketing.Domain.Tickets;
using EventManagement.Ticketing.IntegrationEvents;
using MediatR;

namespace EventManagement.Ticketing.Application.Tickets.CreateTicketBatch;

internal sealed class TicketCreatedDomainEventHandler(ISender sender, IEventBus eventBus)
    : DomainEventHandler<TicketCreatedDomainEvent>
{
    public override async Task Handle(
        TicketCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
       var result = await sender.Send(
            new GetTicketByIdQuery(domainEvent.TicketId),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new InternalServerException(nameof(GetTicketByIdQuery), result.Error);
        }

        await eventBus.PublishAsync(
            new TicketIssuedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOn,
                result.Value.Id,
                result.Value.CustomerId,
                result.Value.EventId,
                result.Value.Code),
            cancellationToken);
    }
}

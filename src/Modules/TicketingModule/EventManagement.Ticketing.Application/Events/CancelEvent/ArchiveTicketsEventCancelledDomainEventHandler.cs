
using EventManagement.Common.Application.Exceptions;
using EventManagement.Ticketing.Application.Tickets.ArchiveTicketsForEvent;
using EventManagement.Ticketing.Domain.Events;
using MediatR;

namespace EventManagement.Ticketing.Application.Events.CancelEvent
{
    internal class ArchiveTicketsEventCancelledDomainEventHandler(
        ISender sender) : DomainEventHandler<EventCancelledDomainEvent>
    {
        public override async Task Handle(EventCancelledDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new ArchiveTicketsForEventCommand(domainEvent.EventId), cancellationToken);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(ArchiveTicketsForEventCommand), result.Error);
            }

        }
    }
}

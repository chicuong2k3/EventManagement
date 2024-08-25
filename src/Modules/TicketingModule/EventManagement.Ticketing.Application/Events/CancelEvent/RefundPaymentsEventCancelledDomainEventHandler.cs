using EventManagement.Common.Application.Exceptions;
using EventManagement.Ticketing.Application.Payments.RefundPaymentsForEvent;
using EventManagement.Ticketing.Domain.Events;
using MediatR;

namespace EventManagement.Ticketing.Application.Events.CancelEvent
{
    internal class RefundPaymentsEventCancelledDomainEventHandler(
        ISender sender) : DomainEventHandler<EventCancelledDomainEvent>
    {
        public override async Task Handle(EventCancelledDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new RefundPaymentsForEventCommand(domainEvent.EventId), cancellationToken);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(RefundPaymentsForEventCommand), result.Error);
            }

        }
    }
}

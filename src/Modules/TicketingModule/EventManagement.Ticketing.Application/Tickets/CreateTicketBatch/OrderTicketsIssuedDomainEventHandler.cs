using EventManagement.Common.Application.Exceptions;
using EventManagement.Ticketing.Application.Tickets;
using EventManagement.Ticketing.Domain.Orders;
using MediatR;

namespace Evently.Modules.Ticketing.Application.Tickets.CreateTicketBatch;

internal sealed class OrderTicketsIssuedDomainEventHandler(ISender sender)
    : DomainEventHandler<OrderTicketsIssuedDomainEvent>
{
    public override async Task Handle(
        OrderTicketsIssuedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new GetTicketsForOrderQuery(domainEvent.OrderId), cancellationToken);

        if (result.IsFailure)
        {
            throw new InternalServerException(nameof(GetTicketsForOrderQuery), result.Error);
        }

        // Send ticket confirmation notification.
    }
}

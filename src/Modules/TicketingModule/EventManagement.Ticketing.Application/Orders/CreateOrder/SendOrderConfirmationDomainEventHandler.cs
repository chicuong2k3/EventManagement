using EventManagement.Common.Application.Exceptions;
using EventManagement.Ticketing.Application.Tickets;
using EventManagement.Ticketing.Domain.Orders;
using MediatR;

namespace EventManagement.Ticketing.Application.Orders.CreateOrder
{
    internal sealed class SendOrderConfirmationDomainEventHandler(ISender sender) 
        : DomainEventHandler<OrderCreatedDomainEvent>
    {
        public override async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetOrderByIdQuery(domainEvent.OrderId), cancellationToken);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(GetOrderByIdQuery), result.Error);
            }

            // send order confirmation motification
        }
    }
}

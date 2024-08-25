using EventManagement.Common.Application.Exceptions;
using EventManagement.Ticketing.Application.Tickets.CreateTicketBatch;
using EventManagement.Ticketing.Domain.Orders;
using MediatR;

namespace EventManagement.Ticketing.Application.Orders.CreateOrder
{
    internal sealed class CreateTicketsDomainEventHandler(ISender sender) 
        : DomainEventHandler<OrderCreatedDomainEvent>
    {
        public override async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new CreateTicketBatchCommand(domainEvent.OrderId), cancellationToken);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(CreateTicketBatchCommand), result.Error);
            }
        }
    }
}

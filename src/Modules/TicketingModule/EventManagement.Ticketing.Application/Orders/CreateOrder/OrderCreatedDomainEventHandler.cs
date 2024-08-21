using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Exceptions;
using EventManagement.Ticketing.Domain.Orders;
using EventManagement.Ticketing.IntegrationEvents;
using MediatR;

namespace EventManagement.Ticketing.Application.Orders.CreateOrder
{
    internal sealed class OrderCreatedDomainEventHandler(
        IEventBus eventBus,
        ISender sender)
        : DomainEventHandler<OrderCreatedDomainEvent>
    {
        public override async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetOrderByIdQuery(domainEvent.OrderId), cancellationToken);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(GetOrderByIdQuery), result.Error);
            }

            await eventBus.PublishAsync(new OrderCreatedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOn,
                result.Value.Id,
                result.Value.CustomerId,
                result.Value.TotalPrice,
                result.Value.CreatedAt,
                result.Value.OrderItems.Select(x => new OrderItemModel
                {
                    Id = x.OrderItemId,
                    OrderId = x.OrderId,
                    TicketTypeId = x.TicketTypeId,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    Price = x.Price,
                    Currency = x.Currency
                }).ToList()
            ), cancellationToken);
        }
    }
}

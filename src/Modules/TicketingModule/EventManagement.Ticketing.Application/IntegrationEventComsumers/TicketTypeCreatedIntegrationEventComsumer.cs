using EventManagement.Common.Application.Exceptions;
using EventManagement.Events.IntegrationEvents;
using EventManagement.Ticketing.Application.UseCases.TicketTypes;
using MediatR;

namespace EventManagement.Ticketing.Application.IntegrationEventComsumers
{
    internal class TicketTypeCreatedIntegrationEventComsumer(ISender sender)
                : IConsumer<TicketTypeCreatedIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<TicketTypeCreatedIntegrationEvent> context)
        {
            var command = new CreateTicketTypeCommand(
                context.Message.Id,
                context.Message.EventId,
                context.Message.Name,
                context.Message.Price,
                context.Message.Currency,
                context.Message.Quantity);

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                throw new InternalServerException(nameof(CreateTicketTypeCommand), result.Error);
            }
        }
    }
}

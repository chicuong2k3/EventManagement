using EventManagement.Events.Application.UseCases.Tickets;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Events.Api.Endpoints.TicketEndpoints;

public sealed record UpdateTicketPriceRequest(
    decimal Price    
);
public sealed class UpdateTicketPriceEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/tickets/{id}/price", async (Guid id, [FromBody] UpdateTicketPriceRequest request, ISender sender) =>
        {
            var command = new UpdateTicketPriceCommand(id, request.Price);

            var response = await sender.Send(command);

            return response.Match(Results.NoContent);

        })
        .WithName("UpdateTicketPrice")
        .WithTags(SwaggerTags.Tickets)
        .WithOpenApi();
    }
}

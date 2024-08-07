
namespace EventManagement.Api.Endpoints.TicketTypeEndpoints;

public sealed record UpdateTicketTypePriceRequest(
    decimal Price
);
public sealed class UpdateTicketTypePriceEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/ticket-types/{id}/price", async (Guid id, [FromBody] UpdateTicketTypePriceRequest request, ISender sender) =>
        {
            var command = new UpdateTicketTypePriceCommand(id, request.Price);

            var response = await sender.Send(command);

            return response.Match(Results.NoContent);

        })
        .WithName("UpdateTicketTypePrice")
        .WithTags(SwaggerTags.TicketTypes)
        .WithOpenApi();
    }
}

using EventManagement.Ticketing.Application.UseCases.Carts.CommandHandlers;

namespace EventManagement.Api.Endpoints.CartEndpoints;

internal sealed record AddItemToCartRequest(
    Guid CustomerId,
    Guid TicketTypeId,
    int Quantity
);
public sealed class AddItemToCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/carts", async ([FromBody] AddItemToCartRequest request, ISender sender) =>
        {
            var command = new AddItemToCartCommand(
                request.CustomerId,
                request.TicketTypeId,
                request.Quantity);

            var response = await sender.Send(command);

            return response.Match(Results.NoContent);

        })
        .WithName("AddItemToCart")
        .WithTags(SwaggerTags.Carts)
        .WithOpenApi();
    }
}

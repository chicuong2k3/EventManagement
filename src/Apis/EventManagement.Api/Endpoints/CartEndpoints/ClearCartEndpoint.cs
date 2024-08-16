using EventManagement.Ticketing.Application.UseCases.Carts.CommandHandlers;

namespace EventManagement.Api.Endpoints.CartEndpoints;

public sealed class ClearCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/carts/{customerId}/clear", async (Guid customerId, ISender sender) =>
        {
            var command = new ClearCartCommand(customerId);

            var response = await sender.Send(command);

            return response.Match(Results.NoContent);

        })
        .WithName("ClearCart")
        .WithTags(SwaggerTags.Carts)
        .WithOpenApi();
    }
}

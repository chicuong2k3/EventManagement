using EventManagement.Ticketing.Application.UseCases.Carts.CommandHandlers;

namespace EventManagement.Api.Endpoints.CartEndpoints;

public sealed class GetCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/carts/{customerId}", async (Guid customerId, ISender sender) =>
        {
            var command = new GetCartQuery(customerId);

            var response = await sender.Send(command);

            return response.Match(Results.Ok);

        })
        .WithName("GetCart")
        .WithTags(SwaggerTags.Carts)
        .WithOpenApi();
    }
}

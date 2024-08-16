using EventManagement.Ticketing.Application.UseCases.Orders;

namespace EventManagement.Api.Endpoints.OrderEndpoints;

public sealed class CreateOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders/customer/{customerId}", async (Guid customerId, ISender sender) =>
        {
            var command = new CreateOrderCommand(customerId);

            var response = await sender.Send(command);

            return response.Match(Results.NoContent);

        })
        .WithName("CreateOrder")
        .WithTags(SwaggerTags.Orders)
        .WithOpenApi();
    }
}

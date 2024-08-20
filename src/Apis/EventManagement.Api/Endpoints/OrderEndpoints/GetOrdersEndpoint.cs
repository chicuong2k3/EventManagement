using EventManagement.Ticketing.Application.Orders;

namespace EventManagement.Api.Endpoints.OrderEndpoints;
public sealed class GetOrdersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/customer/{customerId}", async (Guid customerId, ISender sender) =>
        {
            var response = await sender.Send(new GetOrdersQuery(customerId));

            return response.Match(Results.Ok);
        })
        .WithName("GetOrders")
        .WithTags(SwaggerTags.Orders)
        .WithOpenApi();
    }
}
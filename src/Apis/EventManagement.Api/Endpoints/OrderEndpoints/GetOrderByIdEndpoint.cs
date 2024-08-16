using EventManagement.Ticketing.Application.UseCases.Orders;

namespace EventManagement.Api.Endpoints.OrderEndpoints;
public sealed class GetOrderByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{id}", async (Guid id, ISender sender) =>
        {
            var response = await sender.Send(new GetOrderByIdQuery(id));

            return response.Match(Results.Ok);
        })
        .WithName("GetOrderById")
        .WithTags(SwaggerTags.Orders)
        .WithOpenApi();
    }
}
using EventManagement.Ticketing.Application.Tickets;

namespace EventManagement.Api.Endpoints.TicketEndpoints;
public sealed class GetTicketsForOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tickets/order/{orderId}", async (Guid orderId, ISender sender) =>
        {
            var response = await sender.Send(new GetTicketsForOrderQuery(orderId));

            return response.Match(Results.Ok);
        })
        .WithName("GetTicketsForOrder")
        .WithTags(SwaggerTags.Tickets)
        .WithOpenApi();
    }
}
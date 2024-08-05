using EventManagement.Events.Application.UseCases.Tickets;

namespace EventManagement.Events.Api.Endpoints.TicketEndpoints;
public sealed class GetTicketsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tickets", async (Guid eventId, ISender sender) =>
        {
            var response = await sender.Send(new GetTicketsQuery(eventId));

            return response.Match(Results.Ok);
        })
        .WithName("GetTickets")
        .WithTags(SwaggerTags.Tickets)
        .WithOpenApi();
    }
}
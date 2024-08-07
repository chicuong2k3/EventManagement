namespace EventManagement.Api.Endpoints.TicketTypeEndpoints;
public sealed class GetTicketTypesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/ticket-types", async (Guid eventId, ISender sender) =>
        {
            var response = await sender.Send(new GetTicketTypesQuery(eventId));

            return response.Match(Results.Ok);
        })
        .WithName("GetTicketTypes")
        .WithTags(SwaggerTags.TicketTypes)
        .WithOpenApi();
    }
}
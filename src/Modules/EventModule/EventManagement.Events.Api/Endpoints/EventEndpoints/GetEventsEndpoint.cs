namespace EventManagement.Events.Api.Endpoints.EventEndpoints;
public sealed class GetEventsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/events", async (ISender sender) =>
        {
            var response = await sender.Send(new GetEventsQuery());

            return response.Match(Results.Ok);
        })
        .WithName("GetEvents")
        .WithTags(SwaggerTags.Events)
        .WithOpenApi();
    }
}
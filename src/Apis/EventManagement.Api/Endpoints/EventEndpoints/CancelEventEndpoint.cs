namespace EventManagement.Api.Endpoints.EventEndpoints;

public sealed class CancelEventEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("events/{id}/cancel", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new CancelEventCommand(id));

            return result.Match(Results.NoContent);
        })
        .WithName("CancelEvent")
        .WithTags(SwaggerTags.Events)
        .WithOpenApi();
    }
}

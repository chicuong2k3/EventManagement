using EventManagement.Events.Api.Responses;

namespace EventManagement.Events.Api.Endpoints.EventEndpoints;

public sealed class PublishEventEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("events/{id}/publish", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new PublishEventCommand(id));

            return result.Match(Results.NoContent);
        })
        .WithName("PublishEvent")
        .WithTags(SwaggerTags.Events)
        .WithOpenApi();
    }
}

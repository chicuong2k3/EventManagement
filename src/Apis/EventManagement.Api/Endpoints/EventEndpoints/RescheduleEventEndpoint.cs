namespace EventManagement.Api.Endpoints.EventEndpoints;

internal sealed record RescheduleEventRequest(
    DateTime StartsAt,
    DateTime? EndsAt
);
public sealed class RescheduleEventEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("events/{id}/reschedule", async (Guid id, [FromBody] RescheduleEventRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RescheduleEventCommand(id, request.StartsAt, request.EndsAt));

            return result.Match(Results.NoContent);
        })
        .WithName("RescheduleEvent")
        .WithTags(SwaggerTags.Events)
        .WithOpenApi();
    }
}

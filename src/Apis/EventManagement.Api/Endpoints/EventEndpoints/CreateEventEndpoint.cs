

using EventManagement.Events.Application.Events.CreateEvent;

namespace EventManagement.Api.Endpoints.EventEndpoints;

internal sealed record CreateEventRequest(
    string Title,
    string Description,
    string Location,
    Guid CategoryId,
    DateTime StartsAt,
    DateTime? EndsAt
);
public sealed class CreateEventEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/events", async ([FromBody] CreateEventRequest request, ISender sender) =>
        {
            var command = new CreateEventCommand(
                request.Title,
                request.Description,
                request.Location,
                request.CategoryId,
                request.StartsAt,
                request.EndsAt
            );

            var response = await sender.Send(command);

            return response.Match(Results.Ok);

        })
        .WithName("CreateEvent")
        .WithTags(SwaggerTags.Events)
        .WithOpenApi();
    }
}

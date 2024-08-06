namespace EventManagement.Events.Api.Endpoints.EventEndpoints;
public sealed class GetEventByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/events/{id}", async (Guid id, ISender sender) =>
        {
            var response = await sender.Send(new GetEventByIdQuery(id));

            return response.Match(Results.Ok);
        })
        .WithName("GetEventById")
        .WithTags(SwaggerTags.Events)
        .WithOpenApi();
    }
}
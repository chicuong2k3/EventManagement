namespace EventManagement.Events.Api.Endpoints.EventEndpoints;

internal sealed record SearchEventRequest(
    Guid? CategoryId,
    DateTime? StartDate,
    DateTime? EndDate,
    int Page = 1,
    int PageSize = 10
);
public sealed class SearchEventsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/events/search", async ([AsParameters] SearchEventRequest request, ISender sender) =>
        {
            var query = new SearchEventsQuery(
                request.CategoryId, 
                request.StartDate, 
                request.EndDate,
                request.Page,
                request.PageSize);

            var response = await sender.Send(query);

            return response.Match(Results.Ok);
        })
        .WithName("SearchEvents")
        .WithTags(SwaggerTags.Events)
        .WithOpenApi();
    }
}
using EventManagement.Common.Application.Caching;

namespace EventManagement.Api.Endpoints.CategoryEndpoints;
public sealed class GetCategoriesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/categories", async (ISender sender, ICacheService cacheService) =>
        {
            var cachedResponse = await cacheService.GetAsync<GetCategoriesResponse>("categories");

            if (cachedResponse != null)
            {
                return Results.Ok(cachedResponse);
            }

            var response = await sender.Send(new GetCategoriesQuery());

            if (response.IsSuccess)
            {
                await cacheService.SetAsync("categories", response.Value);
            }

            return response.Match(Results.Ok);
        })
        .RequireAuthorization()
        .WithName("GetCategories")
        .WithTags(SwaggerTags.Categories)
        .WithOpenApi();
    }
}
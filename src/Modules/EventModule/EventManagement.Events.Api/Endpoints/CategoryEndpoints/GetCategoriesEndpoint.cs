using EventManagement.Events.Application.UseCases.Categories;

namespace EventManagement.Events.Api.Endpoints.CategoryEndpoints;
public sealed class GetCategoriesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/categories", async (ISender sender) =>
        {
            var response = await sender.Send(new GetCategoriesQuery());

            return response.Match(Results.Ok);
        })
        .WithName("GetCategories")
        .WithTags(SwaggerTags.Categories)
        .WithOpenApi();
    }
}
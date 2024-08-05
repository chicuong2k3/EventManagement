using EventManagement.Events.Application.UseCases.Categories;

namespace EventManagement.Events.Api.Endpoints.CategoryEndpoints;
public sealed class GetCategoryByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/categories/{id}", async (Guid id, ISender sender) =>
        {
            var response = await sender.Send(new GetCategoryByIdQuery(id));

            return response.Match(Results.Ok);
        })
        .WithName("GetCategoryById")
        .WithTags(SwaggerTags.Categories)
        .WithOpenApi();
    }
}
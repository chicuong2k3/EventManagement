using EventManagement.Events.Application.UseCases.Categories;

namespace EventManagement.Events.Api.Endpoints.CategoryEndpoints;

internal sealed record CreateCategoryRequest(
    string Name
);
public sealed class CreateCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/categories", async ([FromBody] CreateCategoryRequest request, ISender sender) =>
        {
            var command = new CreateCategoryCommand(request.Name);

            var response = await sender.Send(command);

            return response.Match(Results.Created, $"/categories");

        })
        .WithName("CreateCategory")
        .WithTags(SwaggerTags.Categories)
        .WithOpenApi();
    }
}

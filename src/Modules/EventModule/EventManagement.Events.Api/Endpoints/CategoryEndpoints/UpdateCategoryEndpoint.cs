using EventManagement.Events.Application.UseCases.Categories;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Events.Api.Endpoints.CategoryEndpoints;

internal sealed record UpdateCategoryRequest(
    string Name
);
public sealed class UpdateCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/categories/{id}", async(Guid id, [FromBody] UpdateCategoryRequest request, ISender sender) =>
        {
            var command = new UpdateCategoryCommand(id, request.Name);

            var response = await sender.Send(command);

            return response.Match(Results.NoContent);

        })
        .WithName("UpdateCategory")
        .WithTags(SwaggerTags.Categories)
        .WithOpenApi();
    }
}

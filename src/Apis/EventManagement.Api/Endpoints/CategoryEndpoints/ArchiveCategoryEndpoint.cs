
namespace EventManagement.Api.Endpoints.CategoryEndpoints;

public sealed class ArchiveCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/categories/{id}/archive", async (Guid id, ISender sender) =>
        {
            var command = new ArchiveCategoryCommand(id);

            var response = await sender.Send(command);

            return response.Match(Results.NoContent);

        })
        .WithName("ArchiveCategory")
        .WithTags(SwaggerTags.Categories)
        .WithOpenApi();
    }
}

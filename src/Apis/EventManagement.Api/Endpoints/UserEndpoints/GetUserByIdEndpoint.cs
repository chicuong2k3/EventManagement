using EventManagement.Api.Constants;

namespace EventManagement.Api.Endpoints.UserEndpoints;
public sealed class GetUserByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/{id}/profile", async (Guid id, ISender sender) =>
        {
            var response = await sender.Send(new GetUserByIdQuery(id));

            return response.Match(Results.Ok);
        })
        .WithName("GetUserById")
        .WithTags(SwaggerTags.Users)
        .WithOpenApi();
    }
}
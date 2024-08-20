

using EventManagement.Users.Application.Users.UpdateUser;

namespace EventManagement.Api.Endpoints.UserEndpoints;

internal sealed record UpdateUserProfileRequest(
    string FirstName,
    string LastName
);
public sealed class UpdateUserProfileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/users/{id}/profile", async (Guid id, [FromBody] UpdateUserProfileRequest request, ISender sender) =>
        {
            var command = new UpdateUserProfileCommand(id, request.FirstName, request.LastName);

            var response = await sender.Send(command);

            return response.Match(Results.NoContent);

        })
        .WithName("UpdateUserProfile")
        .WithTags(SwaggerTags.Users)
        .WithOpenApi();
    }
}

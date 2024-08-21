

using EventManagement.Common.Infrastructure.Authentication;
using EventManagement.Users.Application.Users.UpdateUser;
using System.Security.Claims;

namespace EventManagement.Api.Endpoints.UserEndpoints;

internal sealed record UpdateUserProfileRequest(
    string FirstName,
    string LastName
);
public sealed class UpdateUserProfileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/users/profile", async (ClaimsPrincipal claims, [FromBody] UpdateUserProfileRequest request, ISender sender) =>
        {
            var command = new UpdateUserProfileCommand(claims.GetUserId(), request.FirstName, request.LastName);

            var response = await sender.Send(command);

            return response.Match(Results.NoContent);

        })
        .RequireAuthorization("users:update")
        .WithName("UpdateUserProfile")
        .WithTags(SwaggerTags.Users)
        .WithOpenApi();
    }
}

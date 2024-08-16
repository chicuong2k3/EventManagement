using EventManagement.Common.Infrastructure.Authentication;
using System.Security.Claims;

namespace EventManagement.Api.Endpoints.UserEndpoints;
public sealed class GetUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/profile", async (ClaimsPrincipal claims, ISender sender) =>
        {
            var response = await sender.Send(new GetUserByIdQuery(claims.GetUserId()));

            return response.Match(Results.Ok);
        })
        .RequireAuthorization("users:read")
        .WithName("GetUser")
        .WithTags(SwaggerTags.Users)
        .WithOpenApi();
    }
}
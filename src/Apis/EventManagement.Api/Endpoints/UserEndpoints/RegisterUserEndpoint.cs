using EventManagement.Api.Constants;

namespace EventManagement.Api.Endpoints.UserEndpoints;

internal sealed record RegisterUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName
);
public sealed class RegisterUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/register", async ([FromBody] RegisterUserRequest request, ISender sender) =>
        {
            var command = new RegisterUserCommand(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName);

            var response = await sender.Send(command);

            return response.Match(Results.Created, $"/users");

        })
        .AllowAnonymous()
        .WithName("RegisterUser")
        .WithTags(SwaggerTags.Users)
        .WithOpenApi();
    }
}

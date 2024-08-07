namespace EventManagement.Users.UserApi;

public interface IUsersApi
{
    Task<GetUserResponse?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);
}

public sealed record GetUserResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName
);

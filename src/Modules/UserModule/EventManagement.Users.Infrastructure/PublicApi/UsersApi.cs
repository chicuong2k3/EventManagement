
namespace EventManagement.Users.Infrastructure.PublicApi;

internal sealed class UsersApi(ISender sender) : IUsersApi
{
    public async Task<GetUserResponse?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetUserByIdQuery(userId), cancellationToken);
        if (result.IsFailure)
        {
            return null;
        }

        return new GetUserResponse(
            result.Value.Id,
            result.Value.Email,
            result.Value.FirstName,
            result.Value.LastName);
    }
}

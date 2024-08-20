

using EventManagement.Common.Domain;

namespace EventManagement.Users.Infrastructure.Authorization
{
    internal sealed class PermissionService(ISender sender) : IPermissionService
    {
        public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
        {
            return await sender.Send(new GetUserPermissionsQuery(identityId));
        }
    }
}

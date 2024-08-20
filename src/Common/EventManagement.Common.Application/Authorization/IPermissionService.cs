using EventManagement.Common.Domain;

namespace EventManagement.Common.Application.Authorization
{
    public interface IPermissionService
    {
        Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId);
    }
}

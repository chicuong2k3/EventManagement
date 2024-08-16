using EventManagement.Common.Domain.Results;

namespace EventManagement.Common.Application.Authorization
{
    public interface IPermissionService
    {
        Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId);
    }
}

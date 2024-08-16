using System.Security.Claims;

namespace EventManagement.Common.Infrastructure.Authentication
{
    public static class ClaimsPrincipleExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal? principal)
        {
            var userId = principal?.FindFirst(CustomClaims.Sub)?.Value;

            return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId 
                : throw new InvalidOperationException("User identifier is unavailable.");
        }

        public static string GetIdentityId(this ClaimsPrincipal? principal)
        {
            return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                throw new InvalidOperationException("User identity is unavailable.");
        }

        public static HashSet<string> GetPermissions(this ClaimsPrincipal? principal)
        {
            var permissionClaims = principal?.FindAll(CustomClaims.Permission)
                ?? throw new InvalidOperationException("Permissions are unavailable.");

            return permissionClaims.Select(x => x.Value).ToHashSet();
        }
    }
}

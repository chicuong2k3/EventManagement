using EventManagement.Common.Domain;
using EventManagement.Users.Application.Abstractions.Identity;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EventManagement.Users.Infrastructure.Identity
{
    internal class IdentityProviderService(
        KeyCloakClient keyCloakClient,
        ILogger<IdentityProviderService> logger)
        : IIdentityProviderService
    {
        const string PasswordCredentialType = "password";
        public async Task<Result<string>> RegisterUserAsync(string email, string password, string firstName, string lastName, CancellationToken cancellationToken = default)
        {
            var userRepresentation = new UserRepresentation(
                email, 
                email,
                firstName, 
                lastName, 
                true, 
                true,
                [new CredentialRepresentation(PasswordCredentialType, password, false)]);

            try
            {
                var identityId = await keyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);

                return identityId;
            }
            catch (HttpRequestException e) when(e.StatusCode == HttpStatusCode.Conflict)
            {
                logger.LogError(e, "User registration failed.");
                return Result.Failure<string>(IdentityProviderErrors.EmailNotUnique);
            }
        }
    }
}

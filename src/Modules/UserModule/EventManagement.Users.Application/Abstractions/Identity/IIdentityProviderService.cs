using EventManagement.Common.Domain;

namespace EventManagement.Users.Application.Abstractions.Identity;

public interface IIdentityProviderService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="cancellation"></param>
    /// <returns>Identity of the user provided by the external identity provider</returns>
    Task<Result<string>> RegisterUserAsync(
        string email, 
        string password,
        string firstName,
        string lastName,
        CancellationToken cancellation = default);
}

namespace EventManagement.Users.Application.Abstractions.Identity;

public class IdentityProviderErrors
{
    public static readonly Error EmailNotUnique =
        Error.Conflict("Identity.EmailNotUnique", "The specified email is not unique.");
}
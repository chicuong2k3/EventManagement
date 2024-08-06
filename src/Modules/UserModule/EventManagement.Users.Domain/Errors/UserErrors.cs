namespace EventManagement.Users.Domain.Errors
{
    public static class UserErrors
    {
        public static Error NotFound(Guid userId) =>
            Error.NotFound("User.NotFound", $"The user with the identifier {userId} was not found");

        public static Error NotFound(string identityId) =>
            Error.NotFound("User.NotFound", $"The user with the IDP identifier {identityId} was not found");
    }

}

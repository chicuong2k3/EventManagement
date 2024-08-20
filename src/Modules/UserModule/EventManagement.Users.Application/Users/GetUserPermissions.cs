
using Dapper;
using EventManagement.Common.Application.Authorization;

namespace EventManagement.Users.Application.Users;

internal sealed class UserPermission
{
    internal Guid UserId { get; init; }
    internal string Permission { get; init; }
}
public sealed record GetUserPermissionsQuery(string IdentityId) : IQuery<PermissionsResponse>;

internal sealed class GetUserPermissionsQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetUserPermissionsQuery, PermissionsResponse>
{
    public async Task<Result<PermissionsResponse>> Handle(GetUserPermissionsQuery query, CancellationToken cancellationToken)
    {
        await using (var connection = await dbConnectionFactory.OpenConnectionAsync())
        {
            const string Sql =
               $"""
                SELECT DISTINCT
                    u.id AS {nameof(UserPermission.UserId)},
                    rp.permission_code AS {nameof(UserPermission.Permission)}
                FROM users.users u
                JOIN users.user_roles ur ON ur.user_id = u.id
                JOIN users.role_permissions rp ON rp.role_name = ur.role_name
                WHERE u.identity_id = @IdentityId
                """
            ;

            var permissions = (await connection.QueryAsync<UserPermission>(Sql, query)).AsList();
            if (!permissions.Any())
            {
                return Result.Failure<PermissionsResponse>(UserErrors.NotFound(query.IdentityId));
            }

            return new PermissionsResponse(
                permissions[0].UserId,
                permissions.Select(x => x.Permission).ToHashSet());
        }
    }
}
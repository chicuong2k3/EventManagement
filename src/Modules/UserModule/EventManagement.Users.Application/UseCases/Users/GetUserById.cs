
using Dapper;

namespace EventManagement.Users.Application.UseCases.Users;

public sealed record GetUserByIdResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName
);

public sealed record GetUserByIdQuery(Guid Id) : IQuery<GetUserByIdResponse>;

internal sealed class GetUserByIdQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    public async Task<Result<GetUserByIdResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        await using (var connection = await dbConnectionFactory.OpenConnectionAsync())
        {
            const string Sql =
               $"""
                SELECT
                    id AS {nameof(GetUserByIdResponse.Id)},
                    email AS {nameof(GetUserByIdResponse.Email)},
                    first_name AS {nameof(GetUserByIdResponse.FirstName)},
                    last_name AS {nameof(GetUserByIdResponse.LastName)}
                FROM users.users
                WHERE id = @Id
                """
            ;

            var user = await connection.QuerySingleOrDefaultAsync<GetUserByIdResponse>(Sql, query);
            if (user == null)
            {
                return Result.Failure<GetUserByIdResponse>(UserErrors.NotFound(query.Id));
            }

            return user;
        }
    }
}

using Dapper;

namespace EventManagement.Events.Application.UseCases.Categories;

public sealed record GetCategoryByIdResponse(
    Guid Id,
    string Name,
    bool IsArchived
);

public sealed record GetCategoryByIdQuery(Guid Id) : IQuery<GetCategoryByIdResponse>;

internal sealed class GetCategoryByIdQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetCategoryByIdQuery, GetCategoryByIdResponse>
{
    public async Task<Result<GetCategoryByIdResponse>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        await using (var connection = await dbConnectionFactory.OpenConnectionAsync())
        {
            const string Sql =
               $"""
                SELECT
                    id AS {nameof(GetCategoryByIdResponse.Id)},
                    name AS {nameof(GetCategoryByIdResponse.Name)},
                    is_archived AS {nameof(GetCategoryByIdResponse.IsArchived)}
                FROM events.categories
                WHERE id = @Id
                """
            ;

            var category = await connection.QueryFirstOrDefaultAsync<GetCategoryByIdResponse>(Sql, query);
            if (category == null)
            {
                return Result.Failure<GetCategoryByIdResponse>(CategoryErrors.NotFound(query.Id));
            }

            return category;
        }
    }
}
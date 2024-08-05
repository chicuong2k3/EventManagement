using Dapper;

namespace EventManagement.Events.Application.UseCases.Categories;

public sealed record CategoryResponse(
    Guid Id,
    string Name,
    bool IsArchived);
public sealed record GetCategoriesResponse(
    IReadOnlyCollection<CategoryResponse> Data
);

public sealed record GetCategoriesQuery(

) : IQuery<GetCategoriesResponse>;
internal sealed class GetCategoriesQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetCategoriesQuery, GetCategoriesResponse>
{
    public async Task<Result<GetCategoriesResponse>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        await using (var connection = await dbConnectionFactory.OpenConnectionAsync())
        {
            const string Sql =
               $"""
                SELECT
                    id AS {nameof(CategoryResponse.Id)},
                    name AS {nameof(CategoryResponse.Name)},
                    is_archived AS {nameof(CategoryResponse.IsArchived)}
                FROM events.categories
                """;

            var result = (await connection.QueryAsync<CategoryResponse>(Sql, query)).AsList();

            return new GetCategoriesResponse(result);
        }
    }
}

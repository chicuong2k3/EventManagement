using Dapper;
using EventManagement.Common.Domain;
using EventManagement.Events.Domain.Events;
using System.Data.Common;

namespace EventManagement.Events.Application.Events;

public sealed record SearchEventsResponse(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<EventResponse> Data
);

public sealed record SearchEventsQuery(
    Guid? CategoryId,
    DateTime? StartDate,
    DateTime? EndDate,
    int Page,
    int PageSize
) : IQuery<SearchEventsResponse>;



internal sealed class SearchEventsQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<SearchEventsQuery, SearchEventsResponse>
{
    public async Task<Result<SearchEventsResponse>> Handle(SearchEventsQuery query, CancellationToken cancellationToken)
    {
        await using (var connection = await dbConnectionFactory.OpenConnectionAsync())
        {
            const string Sql =
               $"""
                SELECT 
                    id AS {nameof(EventResponse.Id)},
                    title AS {nameof(EventResponse.Title)},
                    description AS {nameof(EventResponse.Description)},
                    location AS {nameof(EventResponse.Location)},
                    category_id AS {nameof(EventResponse.CategoryId)},
                    starts_at AS {nameof(EventResponse.StartsAt)},
                    ends_at AS {nameof(EventResponse.EndsAt)}
                FROM events.events
                """;

            var parameters = new SearchEventsParameters(
                (int)EventStatus.Published,
                query.CategoryId,
                query.StartDate?.Date,
                query.EndDate?.Date,
                query.PageSize,
                (query.Page - 1) * query.PageSize
            );

            var events = await GetEventsAsync(connection, parameters);

            var totalCount = await CountEventsAsync(connection, parameters);

            return new SearchEventsResponse(query.Page, query.PageSize, totalCount, events);
        }
    }


    private sealed record SearchEventsParameters(
        int Status,
        Guid? CategoryId,
        DateTime? StartDate,
        DateTime? EndDate,
        int Take,
        int Skip
    );

    private static async Task<int> CountEventsAsync(DbConnection connection, SearchEventsParameters parameters)
    {
        const string Sql =
            """
            SELECT COUNT(*)
            FROM events.events
            WHERE
               status = @Status AND
               (@CategoryId IS NULL OR category_id = @CategoryId) AND
               (@StartDate::timestamp IS NULL OR starts_at >= @StartDate::timestamp) AND
               (@EndDate::timestamp IS NULL OR ends_at >= @EndDate::timestamp)
            """;

        int totalCount = await connection.ExecuteScalarAsync<int>(Sql, parameters);

        return totalCount;
    }

    private static async Task<IReadOnlyCollection<EventResponse>> GetEventsAsync(
        DbConnection connection,
        SearchEventsParameters parameters)
    {
        const string Sql =
            $"""
             SELECT
                 id AS {nameof(EventResponse.Id)},
                 title AS {nameof(EventResponse.Title)},
                 description AS {nameof(EventResponse.Description)},
                 location AS {nameof(EventResponse.Location)},
                 category_id AS {nameof(EventResponse.CategoryId)},
                 starts_at AS {nameof(EventResponse.StartsAt)},
                 ends_at AS {nameof(EventResponse.EndsAt)}
             FROM events.events
             WHERE
                status = @Status AND
                (@CategoryId IS NULL OR category_id = @CategoryId) AND
                (@StartDate::timestamp IS NULL OR starts_at >= @StartDate::timestamp) AND
                (@EndDate::timestamp IS NULL OR ends_at >= @EndDate::timestamp)
             ORDER BY starts_at
             OFFSET @Skip
             LIMIT @Take
             """;

        List<EventResponse> events = (await connection.QueryAsync<EventResponse>(Sql, parameters)).AsList();

        return events;
    }

}

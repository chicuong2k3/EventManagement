using Dapper;
using EventManagement.Common.Domain;

namespace EventManagement.Events.Application.Events;

public sealed record EventResponse(
    Guid Id,
    string Title,
    string Description,
    string Location,
    Guid CategoryId,
    DateTime StartsAt,
    DateTime? EndsAt);
public sealed record GetEventsResponse(
    IReadOnlyCollection<EventResponse> Data
);

public sealed record GetEventsQuery(

) : IQuery<GetEventsResponse>;
internal sealed class GetEventsQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetEventsQuery, GetEventsResponse>
{
    public async Task<Result<GetEventsResponse>> Handle(GetEventsQuery query, CancellationToken cancellationToken)
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

            var result = (await connection.QueryAsync<EventResponse>(Sql, query)).AsList();

            return new GetEventsResponse(result);
        }
    }
}

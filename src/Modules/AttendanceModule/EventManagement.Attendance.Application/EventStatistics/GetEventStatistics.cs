

using Dapper;
using EventManagement.Common.Application.Data;

namespace EventManagement.Attendance.Application.EventStatistics;

public sealed record EventStatisticsResponse(
    Guid EventId, 
    string Title, 
    string Description, 
    string Location, 
    DateTime StartsAt, 
    DateTime? EndsAt, 
    int TicketsSold, 
    int AttendeesCheckedIn, 
    List<string> DuplicateCheckInTickets, 
    List<string> InvalidCheckInTickets);
public sealed record GetEventStatisticsQuery(Guid EventId) : IQuery<EventStatisticsResponse>;
internal sealed class GetEventStatisticsQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetEventStatisticsQuery, EventStatisticsResponse>
{
    public async Task<Result<EventStatisticsResponse>> Handle(GetEventStatisticsQuery query, CancellationToken cancellationToken)
    {
        await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();

        string sql =
            $"""
            SELECT 
                event_id AS {nameof(EventStatisticsResponse.EventId)},
                title AS {nameof(EventStatisticsResponse.Title)},
                description AS {nameof(EventStatisticsResponse.Description)},
                location AS {nameof(EventStatisticsResponse.Location)},
                starts_at AS {nameof(EventStatisticsResponse.StartsAt)},
                ends_at AS {nameof(EventStatisticsResponse.EndsAt)},
                tickets_sold AS {nameof(EventStatisticsResponse.TicketsSold)},
                attendees_checked_in AS {nameof(EventStatisticsResponse.AttendeesCheckedIn)},
                duplicate_check_in_tickets AS {nameof(EventStatisticsResponse.DuplicateCheckInTickets)},
                invalid_check_in_tickets AS {nameof(EventStatisticsResponse.InvalidCheckInTickets)}
            FROM attendance.event_statistics
            WHERE event_id = @EventId
            """;

        var eventStatistics = await dbConnection.QuerySingleOrDefaultAsync<EventStatisticsResponse>(sql, query);

        if (eventStatistics == null)
        {
            return Result.Failure<EventStatisticsResponse>(EventErrors.NotFound(query.EventId));
        }

        return eventStatistics; 
    }
}

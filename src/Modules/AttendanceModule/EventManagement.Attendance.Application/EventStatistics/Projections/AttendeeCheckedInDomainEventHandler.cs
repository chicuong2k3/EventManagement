using Dapper;
using EventManagement.Common.Application.Data;

namespace EventManagement.Attendance.Application.EventStatistics.Projections
{
    internal class AttendeeCheckedInDomainEventHandler(
    IDbConnectionFactory dbConnectionFactory)
        : DomainEventHandler<AttendeeCheckedInDomainEvent>
    {
        public override async Task Handle(AttendeeCheckedInDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();

            string sql =
                """
                UPDATE attendance.event_statistics es
                SET
                    attendees_checked_in = (
                        SELECT COUNT(*)
                        FROM attendance.tickets t
                        WHERE t.event_id = es.event_id
                        AND t.used_at IS NOT NULL
                    )
                WHERE es.event_id = @EventId
                """;

            await dbConnection.ExecuteAsync(sql, domainEvent);
        }
    }
}

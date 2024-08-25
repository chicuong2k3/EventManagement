using Dapper;
using EventManagement.Common.Application.Data;

namespace EventManagement.Attendance.Application.EventStatistics.Projections
{
    internal class DuplicateCheckInAttemptedDomainEventHandler(
    IDbConnectionFactory dbConnectionFactory)
        : DomainEventHandler<DuplicateCheckInAttemptedDomainEvent>
    {
        public override async Task Handle(DuplicateCheckInAttemptedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();

            string sql =
                """
                UPDATE attendance.event_statistics
                SET duplicate_check_in_tickets = array_append(duplicate_check_in_tickets, @TicketCode)
                WHERE event_id = @EventId
                """;

            await dbConnection.ExecuteAsync(sql, domainEvent);
        }
    }
}

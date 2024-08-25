using Dapper;
using EventManagement.Common.Application.Data;

namespace EventManagement.Attendance.Application.EventStatistics.Projections
{
    internal class InvalidCheckInAttemptedDomainEventHandler(
    IDbConnectionFactory dbConnectionFactory)
        : DomainEventHandler<InvalidCheckInAttemptedDomainEvent>
    {
        public override async Task Handle(InvalidCheckInAttemptedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();

            string sql =
                """
                UPDATE attendance.event_statistics
                SET invalid_check_in_tickets = array_append(invalid_check_in_tickets, @TicketCode)
                WHERE event_id = @EventId
                """;

            await dbConnection.ExecuteAsync(sql, domainEvent);
        }
    }
}

using Dapper;
using EventManagement.Common.Application.Data;

namespace EventManagement.Attendance.Application.EventStatistics.Projections
{
    internal class TicketCreatedDomainEventHandler(
    IDbConnectionFactory dbConnectionFactory)
        : DomainEventHandler<TicketCreatedDomainEvent>
    {
        public override async Task Handle(TicketCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();

            string sql =
                """
                UPDATE attendance.event_statistics es
                SET
                    tickets_sold = (
                        SELECT COUNT(*)
                        FROM attendance.tickets t
                        WHERE t.event_id = es.event_id
                    )
                WHERE es.event_id = @EventId
                """;

            await dbConnection.ExecuteAsync(sql, domainEvent);
        }
    }
}


using Dapper;
using EventManagement.Common.Application.Data;

namespace EventManagement.Attendance.Application.EventStatistics.Projections
{
    internal sealed class EventCreatedDomainEventHandler(
    IDbConnectionFactory dbConnectionFactory)
        : DomainEventHandler<EventCreatedDomainEvent>
    {
        public override async Task Handle(EventCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();

            string sql =
                """
                INSERT INTO attendance.event_statistics(
                    event_id,
                    title,
                    description,
                    location,
                    starts_at,
                    ends_at,
                    tickets_sold,
                    attendees_checked_in,
                    duplicate_check_in_tickets,
                    invalid_check_in_tickets
                )
                VALUES (
                    @EventId,
                    @Title,
                    @Description,
                    @Location,
                    @StartsAt,
                    @EndsAt,
                    @TicketsSold,
                    @AttendeesCheckedIn,
                    @DuplicateCheckInTickets,
                    @InvalidCheckInTickets
                )
                """;

            await dbConnection.ExecuteAsync(sql, new
            {
                domainEvent.EventId,
                domainEvent.Title,
                domainEvent.Description,
                domainEvent.Location,
                domainEvent.StartsAt,
                domainEvent.EndsAt,
                TicketsSold = 0,
                AttendeesCheckedIn = 0,
                DuplicateCheckInTickets = Array.Empty<string>(),
                InvalidCheckInTickets = Array.Empty<string>()
            });
        }
    }
}

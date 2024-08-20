
using Dapper;
using EventManagement.Common.Domain;
using EventManagement.Events.Domain.Events;

namespace EventManagement.Events.Application.Events;

public sealed record GetEventByIdResponse(
    Guid Id,
    string Title,
    string Description,
    string Location,
    Guid CategoryId,
    DateTime StartsAt,
    DateTime? EndsAt
)
{
    public List<TicketResponse> Tickets { get; } = [];
}
public sealed record TicketResponse(
    Guid TicketId,
    string Name,
    decimal Price,
    string Currency,
    int Quantity);


public sealed record GetEventByIdQuery(Guid Id) : IQuery<GetEventByIdResponse>;

internal sealed class GetEventByIdQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetEventByIdQuery, GetEventByIdResponse>
{
    public async Task<Result<GetEventByIdResponse>> Handle(GetEventByIdQuery query, CancellationToken cancellationToken)
    {
        await using (var connection = await dbConnectionFactory.OpenConnectionAsync())
        {
            const string Sql =
               $"""
                SELECT 
                    e.id AS {nameof(GetEventByIdResponse.Id)},
                    e.title AS {nameof(GetEventByIdResponse.Title)},
                    e.description AS {nameof(GetEventByIdResponse.Description)},
                    e.location AS {nameof(GetEventByIdResponse.Location)},
                    e.category_id AS {nameof(GetEventByIdResponse.CategoryId)},
                    e.starts_at AS {nameof(GetEventByIdResponse.StartsAt)},
                    e.ends_at AS {nameof(GetEventByIdResponse.EndsAt)},
                    t.id AS {nameof(TicketResponse.TicketId)},
                    t.name AS {nameof(TicketResponse.Name)},
                    t.price AS {nameof(TicketResponse.Price)},
                    t.currency AS {nameof(TicketResponse.Currency)},
                    t.quantity AS {nameof(TicketResponse.Quantity)}
                FROM events.events e
                LEFT JOIN events.ticket_types t ON t.event_id = e.id
                WHERE e.id = @Id
                """;

            Dictionary<Guid, GetEventByIdResponse> eventsDictionary = [];

            await connection.QueryAsync<GetEventByIdResponse, TicketResponse, GetEventByIdResponse>(
                Sql,
                (eventEntity, ticket) =>
                {
                    if (eventsDictionary.TryGetValue(eventEntity.Id, out GetEventByIdResponse? existingEvent))
                    {
                        eventEntity = existingEvent;
                    }
                    else
                    {
                        eventsDictionary.Add(eventEntity.Id, eventEntity);
                    }

                    if (ticket != null)
                    {
                        eventEntity.Tickets.Add(ticket);
                    }

                    return eventEntity;
                },
                query,
                splitOn: nameof(TicketResponse.TicketId)
            );

            if (!eventsDictionary.TryGetValue(query.Id, out GetEventByIdResponse? response))
            {
                return Result.Failure<GetEventByIdResponse>(EventErrors.NotFound(query.Id));
            }

            return response;
        }
    }
}
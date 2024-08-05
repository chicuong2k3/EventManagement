
using Dapper;

namespace EventManagement.Events.Application.UseCases.Events;

public sealed record GetEventByIdResponse(
    Guid Id,
    string Title,
    string Description,
    string Location,
    Guid CategoryId,
    DateTime StartsAt,
    DateTime? EndsAt,
    string Status
)
{
    public List<TicketResponse> Tickets { get; } = [];
}
public sealed record TicketResponse(
    Guid TicketId,
    string Name,
    decimal Price,
    string Currency,
    decimal Quantity);


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
                    id AS {nameof(GetEventByIdResponse.Id)},
                    title AS {nameof(GetEventByIdResponse.Title)},
                    description AS {nameof(GetEventByIdResponse.Description)},
                    location AS {nameof(GetEventByIdResponse.Location)},
                    category_id AS {nameof(GetEventByIdResponse.CategoryId)},
                    starts_at AS {nameof(GetEventByIdResponse.StartsAt)},
                    ends_at AS {nameof(GetEventByIdResponse.EndsAt)},
                    t.id AS {nameof(TicketResponse.TicketId)},
                    t.name AS {nameof(TicketResponse.Name)},
                    t.price AS {nameof(TicketResponse.Price)},
                    t.currency AS {nameof(TicketResponse.Currency)},
                    t.quantity AS {nameof(TicketResponse.Quantity)}
                FROM events.events e
                LEFT JOIN events.tickets t ON t.event_id = e.id
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
            splitOn: nameof(TicketResponse.TicketId));

            if (!eventsDictionary.TryGetValue(query.Id, out GetEventByIdResponse? response))
            {
                return Result.Failure<GetEventByIdResponse>(EventErrors.NotFound(query.Id));
            }

            return response;
        }
    }
}
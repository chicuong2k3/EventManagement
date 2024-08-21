
using Dapper;
using EventManagement.Common.Domain;

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
    public List<TicketTypeResponse> TicketTypes { get; } = [];
}
public sealed record TicketTypeResponse(
    Guid TicketTypeId,
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
                    t.id AS {nameof(TicketTypeResponse.TicketTypeId)},
                    t.name AS {nameof(TicketTypeResponse.Name)},
                    t.price AS {nameof(TicketTypeResponse.Price)},
                    t.currency AS {nameof(TicketTypeResponse.Currency)},
                    t.quantity AS {nameof(TicketTypeResponse.Quantity)}
                FROM events.events e
                LEFT JOIN events.ticket_types t ON t.event_id = e.id
                WHERE e.id = @Id
                """;

            Dictionary<Guid, GetEventByIdResponse> eventsDictionary = [];

            await connection.QueryAsync<GetEventByIdResponse, TicketTypeResponse, GetEventByIdResponse>(
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
                        eventEntity.TicketTypes.Add(ticket);
                    }

                    return eventEntity;
                },
                query,
                splitOn: nameof(TicketTypeResponse.TicketTypeId)
            );

            if (!eventsDictionary.TryGetValue(query.Id, out GetEventByIdResponse? response))
            {
                return Result.Failure<GetEventByIdResponse>(EventErrors.NotFound(query.Id));
            }

            return response;
        }
    }
}
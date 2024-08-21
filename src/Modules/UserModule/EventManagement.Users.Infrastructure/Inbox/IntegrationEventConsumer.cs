using Dapper;
using EventManagement.Common.Application.Data;
using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Infrastructure.Inbox;
using EventManagement.Common.Infrastructure.Serialization;
using MassTransit;
using Newtonsoft.Json;

namespace EventManagement.Users.Infrastructure.Inbox
{
    internal sealed class IntegrationEventConsumer<TIntegrationEvent>(IDbConnectionFactory dbConnectionFactory)
        : IConsumer<TIntegrationEvent>
        where TIntegrationEvent : IntegrationEvent
    {
        public async Task Consume(ConsumeContext<TIntegrationEvent> context)
        {
            await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();

            var integrationEvent = context.Message;

            var inboxMessage = new InboxMessage
            {
                Id = integrationEvent.Id,
                Type = integrationEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(integrationEvent, SerializerSettings.Instance),
                OccurredOn = integrationEvent.OccurredOn
            };

            string sql =
                $"""
                INSERT INTO {Schemas.Users}.inbox_messages(id, type, content, occurred_on)
                VALUES (@Id, @Type, @Content::json, @OccurredOn);
                """;

            await dbConnection.ExecuteAsync(sql, inboxMessage);
        }
    }
}

using Dapper;
using EventManagement.Common.Application.Data;
using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Infrastructure.Inbox;
using EventManagement.Common.Infrastructure.Serialization;
using MassTransit;
using Newtonsoft.Json;

namespace EventManagement.Ticketing.Infrastructure.Inbox
{
    internal sealed class IntegrationEventConsumer<TIntegrationEvent>(IDbConnectionFactory dbConnectionFactory)
        : IConsumer<TIntegrationEvent>
        where TIntegrationEvent : IntegrationEvent
    {
        private const string Schema = "ticketing";
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
                INSERT INTO {Schema}.inbox_messages(id, type, content, occurred_on)
                VALUES (@Id, @Type, @Content::json, @OccurredOn);
                """;
                  
            await dbConnection.ExecuteAsync(sql, inboxMessage);
        }
    }
}

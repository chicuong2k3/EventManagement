using Dapper;
using EventManagement.Common.Application.Data;
using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Application.Messaging;
using EventManagement.Common.Domain;
using EventManagement.Common.Infrastructure.Inbox;
using EventManagement.Common.Infrastructure.Outbox;
using System.Data.Common;

namespace EventManagement.Ticketing.Infrastructure.Inbox
{

    internal sealed class IdempotentIntegrationEventHandler<TIntegrationEvent>(
        IIntegrationEventHandler<TIntegrationEvent> integrationEventHandler,
        IDbConnectionFactory dbConnectionFactory) : IntegrationEventHandler<TIntegrationEvent>
        where TIntegrationEvent : IIntegrationEvent
    {
        private const string Schema = "ticketing";
        public override async Task Handle(
            TIntegrationEvent integrationEvent,
            CancellationToken cancellationToken = default)
        {
            await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();

            var inboxMessageConsumer = new InboxMessageConsumer(
                integrationEvent.Id,
                integrationEventHandler.GetType().Name
            );

            if (await InboxConsumerExistsAsync(dbConnection, inboxMessageConsumer))
            {
                return;
            }

            await integrationEventHandler.Handle(integrationEvent, cancellationToken);

            await InsertInboxMessageConsumerAsync(dbConnection, inboxMessageConsumer);
        }

        private async Task<bool> InboxConsumerExistsAsync(DbConnection dbConnection, InboxMessageConsumer inboxMessageConsumer)
        {
            string sql =
                $"""
                SELECT EXISTS (
                    SELECT 1
                    FROM {Schema}.inbox_message_consumers
                    WHERE inbox_message_id = @InboxMessageId
                    AND handler_name = @HandlerName
                )
                """;

            return await dbConnection.ExecuteScalarAsync<bool>(
                sql,
                inboxMessageConsumer
            );
        }

        private async Task InsertInboxMessageConsumerAsync(DbConnection dbConnection, InboxMessageConsumer inboxMessageConsumer)
        {
            string sql =
                $"""
                INSERT INTO {Schema}.inbox_message_consumers(inbox_message_id, handler_name)
                VALUES (@InboxMessageId, @HandlerName)
                """;

            await dbConnection.ExecuteAsync(
                sql,
                inboxMessageConsumer
            );
        }
    }
}

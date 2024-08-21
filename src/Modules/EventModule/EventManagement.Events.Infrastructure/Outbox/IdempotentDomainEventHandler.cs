﻿using Dapper;
using EventManagement.Common.Application.Data;
using EventManagement.Common.Application.Messaging;
using EventManagement.Common.Domain;
using EventManagement.Common.Infrastructure.Inbox;
using EventManagement.Common.Infrastructure.Outbox;
using System.Data.Common;

namespace EventManagement.Events.Infrastructure.Outbox
{

    internal sealed class IdempotentDomainEventHandler<TDomainEvent>(
        IDomainEventHandler<TDomainEvent> domainEventHandler,
        IDbConnectionFactory dbConnectionFactory) : DomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        private const string Schema = "events";
        public override async Task Handle(
            TDomainEvent domainEvent,
            CancellationToken cancellationToken = default)
        {
            await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();

            var outboxMessageConsumer = new OutboxMessageConsumer(
                domainEvent.Id,
                domainEventHandler.GetType().Name
            );

            if (await OutboxConsumerExistsAsync(dbConnection, outboxMessageConsumer))
            {
                return;
            }

            await domainEventHandler.Handle(domainEvent, cancellationToken);

            await InsertOutboxMessageConsumerAsync(dbConnection, outboxMessageConsumer);
        }

        private async Task<bool> OutboxConsumerExistsAsync(DbConnection dbConnection, OutboxMessageConsumer outboxMessageConsumer)
        {
            string sql =
                $"""
                SELECT EXISTS (
                    SELECT 1
                    FROM {Schema}.outbox_message_consumers
                    WHERE outbox_message_id = @OutboxMessageId
                    AND handler_name = @HandlerName
                )
                """;

            return await dbConnection.ExecuteScalarAsync<bool>(
                sql,
                outboxMessageConsumer
            );
        }

        private async Task InsertOutboxMessageConsumerAsync(DbConnection dbConnection, OutboxMessageConsumer outboxMessageConsumer)
        {
            string sql =
                $"""
                INSERT INTO {Schema}.outbox_message_consumers(outbox_message_id, handler_name)
                VALUES (@OutboxMessageId, @HandlerName)
                """;

            await dbConnection.ExecuteAsync(
                sql,
                outboxMessageConsumer
            );
        }
    }
}
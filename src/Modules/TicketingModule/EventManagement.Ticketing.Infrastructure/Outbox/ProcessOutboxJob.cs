using Dapper;
using EventManagement.Common.Application.Data;
using EventManagement.Common.Domain;
using EventManagement.Common.Infrastructure.Serialization;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using System.Data;

namespace EventManagement.Ticketing.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxJob(
    IDbConnectionFactory dbConnectionFactory,
    IServiceScopeFactory serviceScopeFactory,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<ProcessOutboxJob> logger) : IJob
{
    private const string ModuleName = "Ticketing";
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("{ModuleName}: Beginning to process outbox messages.", ModuleName);

        await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();
        await using var dbTransaction = await dbConnection.BeginTransactionAsync();

        var outboxMessages = await GetOutboxMessagesAsync(dbConnection, dbTransaction);

        foreach (var outboxMessage in outboxMessages)
        {
            Exception? exception = null;
            try
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    SerializerSettings.Instance)!;

                using var scope = serviceScopeFactory.CreateScope();

                var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
                await publisher.Publish(domainEvent, context.CancellationToken);
            }
            catch (Exception e)
            {
                logger.LogError(e,
                    "{ModuleName}: Errors occurred while processing outbox message {MessageId}.",
                    ModuleName,
                    outboxMessage.Id);
                exception = e;
            }


            await UpdateOutboxMessagesAsync(dbConnection, dbTransaction, outboxMessage, exception);
        }



        await dbTransaction.CommitAsync();
        logger.LogInformation("{ModuleName}: Completed processing outbox messages.", ModuleName);
    }

    private async Task<IEnumerable<OutboxMessageResponse>> GetOutboxMessagesAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction)
    {
        string sql =
            $"""
            SELECT
                id AS {nameof(OutboxMessageResponse.Id)},
                content AS {nameof(OutboxMessageResponse.Content)}
            FROM ticketing.outbox_messages
            WHERE processed_on IS NULL
            ORDER BY occurred_on
            LIMIT {outboxOptions.Value.BatchSize}
            FOR UPDATE
            """;

        var outboxMessages = await dbConnection.QueryAsync<OutboxMessageResponse>(
            sql,
            transaction: dbTransaction);

        return outboxMessages.ToList();
    }

    private async Task UpdateOutboxMessagesAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        OutboxMessageResponse outboxMessageResponse,
        Exception? exception)
    {
        string sql =
            $"""
            UPDATE ticketing.outbox_messages
            SET processed_on = @ProcessedOn, error = @Error
            WHERE id = @Id
            """;

        await dbConnection.ExecuteAsync(
            sql,
            new
            {
                outboxMessageResponse.Id,
                ProcessedOn = DateTime.UtcNow,
                Error = exception?.ToString()
            },
            transaction: dbTransaction);
    }
}

internal sealed record OutboxMessageResponse(Guid Id, string Content);


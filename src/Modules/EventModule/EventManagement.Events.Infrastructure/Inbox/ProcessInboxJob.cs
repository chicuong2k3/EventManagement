using Dapper;
using EventManagement.Common.Application.Data;
using EventManagement.Common.Application.EventBuses;
using EventManagement.Common.Infrastructure.Inbox;
using EventManagement.Common.Infrastructure.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using System.Data;

namespace EventManagement.Events.Infrastructure.Inbox;

[DisallowConcurrentExecution]
internal sealed class ProcessInboxJob(
    IDbConnectionFactory dbConnectionFactory,
    IServiceScopeFactory serviceScopeFactory,
    IOptions<InboxOptions> inboxOptions,
    ILogger<ProcessInboxJob> logger) : IJob
{
    private const string ModuleName = "Events";
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("{ModuleName}: Beginning to process inbox messages.", ModuleName);

        await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();
        await using var dbTransaction = await dbConnection.BeginTransactionAsync();

        var inboxMessages = await GetInboxMessagesAsync(dbConnection, dbTransaction);

        foreach (var inboxMessage in inboxMessages)
        {
            Exception? exception = null;
            try
            {
                var integrationEvent = JsonConvert.DeserializeObject<IIntegrationEvent>(
                    inboxMessage.Content,
                    SerializerSettings.Instance)!;

                using var scope = serviceScopeFactory.CreateScope();

                var integrationEventHandlers = IntegrationEventHandlerFactory.GetHandlers(
                   integrationEvent.GetType(),
                   scope.ServiceProvider,
                   Application.AssemblyReference.Assembly);

                foreach (var integrationEventHandler in integrationEventHandlers)
                {
                    await integrationEventHandler.Handle(integrationEvent, context.CancellationToken);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e,
                    "{ModuleName}: Errors occurred while processing inbox message {MessageId}.",
                    ModuleName,
                    inboxMessage.Id);
                exception = e;
            }


            await UpdateInboxMessagesAsync(dbConnection, dbTransaction, inboxMessage, exception);
        }



        await dbTransaction.CommitAsync();
        logger.LogInformation("{ModuleName}: Completed processing inbox messages.", ModuleName);
    }

    private async Task<IEnumerable<InboxMessageResponse>> GetInboxMessagesAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction)
    {
        string sql =
            $"""
            SELECT
                id AS {nameof(InboxMessageResponse.Id)},
                content AS {nameof(InboxMessageResponse.Content)}
            FROM {Schemas.Events}.inbox_messages
            WHERE processed_on IS NULL
            ORDER BY occurred_on
            LIMIT {inboxOptions.Value.BatchSize}
            FOR UPDATE
            """;

        var inboxMessages = await dbConnection.QueryAsync<InboxMessageResponse>(
            sql,
            transaction: dbTransaction);

        return inboxMessages.ToList();
    }

    private async Task UpdateInboxMessagesAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        InboxMessageResponse inboxMessageResponse,
        Exception? exception)
    {
        string sql =
            $"""
            UPDATE {Schemas.Events}.inbox_messages
            SET processed_on = @ProcessedOn, error = @Error
            WHERE id = @Id
            """;

        await dbConnection.ExecuteAsync(
            sql,
            new
            {
                inboxMessageResponse.Id,
                ProcessedOn = DateTime.UtcNow,
                Error = exception?.ToString()
            },
            transaction: dbTransaction);
    }
}

internal sealed record InboxMessageResponse(Guid Id, string Content);


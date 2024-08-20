using Microsoft.Extensions.Options;
using Quartz;

namespace EventManagement.Ticketing.Infrastructure.Outbox
{
    internal sealed class ConfigureProcessOutboxJob(IOptions<OutboxOptions> outboxOptions)
        : IConfigureOptions<QuartzOptions>
    {
        private readonly OutboxOptions _outboxOptions = outboxOptions.Value;
        public void Configure(QuartzOptions options)
        {
            var jobName = typeof(ProcessOutboxJob).FullName!;

            options.AddJob<ProcessOutboxJob>(configure => configure.WithIdentity(jobName))
                .AddTrigger(config =>
                {
                    config.ForJob(jobName)
                        .WithSimpleSchedule(schedule =>
                            schedule.WithIntervalInSeconds(_outboxOptions.IntervalInSeconds)
                                .RepeatForever());
                });
        }
    }
}

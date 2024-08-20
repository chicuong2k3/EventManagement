using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Common.Infrastructure.Outbox
{
    public sealed class OutboxMessageConsumerConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
    {
        public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
        {
            builder.ToTable("outbox_message_consumers");
            builder.HasKey(x => new { x.OutboxMessageId, x.HandlerName });
            builder.Property(x => x.HandlerName)
                .HasMaxLength(500);
        }
    }
}

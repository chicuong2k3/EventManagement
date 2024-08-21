
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Common.Infrastructure.Inbox
{
    public sealed class InboxMessageConsumerConfiguration : IEntityTypeConfiguration<InboxMessageConsumer>
    {
        public void Configure(EntityTypeBuilder<InboxMessageConsumer> builder)
        {
            builder.ToTable("inbox_message_consumers");
            builder.HasKey(x => new { x.InboxMessageId, x.HandlerName });
            builder.Property(x => x.HandlerName)
                .HasMaxLength(500);
        }
    }
}

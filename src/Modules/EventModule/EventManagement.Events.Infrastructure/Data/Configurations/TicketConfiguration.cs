using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Events.Infrastructure.Data.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Currency)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne<EventEntity>()
                .WithMany()
                .HasForeignKey(x => x.EventId);
        }
    }
}

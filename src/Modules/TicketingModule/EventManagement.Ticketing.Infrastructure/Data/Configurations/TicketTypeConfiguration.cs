using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Ticketing.Infrastructure.Data.Configurations;

internal sealed class TicketTypeConfiguration : IEntityTypeConfiguration<TicketType>
{
    public void Configure(EntityTypeBuilder<TicketType> builder)
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

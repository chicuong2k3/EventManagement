using EventManagement.Events.Domain.Categories;
using EventManagement.Events.Domain.Events;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Events.Infrastructure.Events
{
    public class EventConfiguration : IEntityTypeConfiguration<EventEntity>
    {
        public void Configure(EntityTypeBuilder<EventEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Location)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(x => x.CategoryId);
        }
    }
}

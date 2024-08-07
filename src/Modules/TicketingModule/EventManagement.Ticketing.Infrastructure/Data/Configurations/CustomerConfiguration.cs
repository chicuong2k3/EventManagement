using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Ticketing.Infrastructure.Data.Configurations;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName)
            .HasMaxLength(100);

        builder.Property(c => c.LastName)
            .HasMaxLength(100);

        builder.Property(c => c.Email)
            .HasMaxLength(100);
    }
}

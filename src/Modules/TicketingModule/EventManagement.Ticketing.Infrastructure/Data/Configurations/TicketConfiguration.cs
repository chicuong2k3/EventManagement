﻿using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Ticketing.Infrastructure.Data.Configurations;

internal sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Code).HasMaxLength(30);

        builder.HasIndex(t => t.Code).IsUnique();

        builder.HasOne<Customer>().WithMany().HasForeignKey(t => t.CustomerId);

        builder.HasOne<Order>().WithMany().HasForeignKey(t => t.OrderId);

        builder.HasOne<EventEntity>().WithMany().HasForeignKey(t => t.EventId);

        builder.HasOne<TicketType>().WithMany().HasForeignKey(t => t.TicketTypeId);
    }
}

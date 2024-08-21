﻿// <auto-generated />
using System;
using EventManagement.Ticketing.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EventManagement.Ticketing.Infrastructure.Data.Migrations
{
    [DbContext(typeof(TicketingDbContext))]
    partial class TicketingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("ticketing")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EventManagement.Common.Infrastructure.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("jsonb")
                        .HasColumnName("content");

                    b.Property<string>("Error")
                        .HasColumnType("text")
                        .HasColumnName("error");

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("occurred_on");

                    b.Property<DateTime?>("ProcessedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("processed_on");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_outbox_messages");

                    b.ToTable("outbox_messages", "ticketing");
                });

            modelBuilder.Entity("EventManagement.Common.Infrastructure.Outbox.OutboxMessageConsumer", b =>
                {
                    b.Property<Guid>("OutboxMessageId")
                        .HasColumnType("uuid")
                        .HasColumnName("outbox_message_id");

                    b.Property<string>("HandlerName")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("handler_name");

                    b.HasKey("OutboxMessageId", "HandlerName")
                        .HasName("pk_outbox_message_consumers");

                    b.ToTable("outbox_message_consumers", "ticketing");
                });

            modelBuilder.Entity("EventManagement.Ticketing.Domain.Customers.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("last_name");

                    b.HasKey("Id")
                        .HasName("pk_customers");

                    b.ToTable("customers", "ticketing");
                });

            modelBuilder.Entity("EventManagement.Ticketing.Domain.Events.EventEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("Cancelled")
                        .HasColumnType("boolean")
                        .HasColumnName("cancelled");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<DateTime?>("EndsAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ends_at");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("location");

                    b.Property<DateTime>("StartsAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("starts_at");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_events");

                    b.ToTable("events", "ticketing");
                });

            modelBuilder.Entity("EventManagement.Ticketing.Domain.Orders.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("currency");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid")
                        .HasColumnName("customer_id");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<bool>("TicketsIssued")
                        .HasColumnType("boolean")
                        .HasColumnName("tickets_issued");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric")
                        .HasColumnName("total_price");

                    b.HasKey("Id")
                        .HasName("pk_orders");

                    b.HasIndex("CustomerId")
                        .HasDatabaseName("ix_orders_customer_id");

                    b.ToTable("orders", "ticketing");
                });

            modelBuilder.Entity("EventManagement.Ticketing.Domain.Orders.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("currency");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("order_id");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<Guid>("TicketTypeId")
                        .HasColumnType("uuid")
                        .HasColumnName("ticket_type_id");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("numeric")
                        .HasColumnName("unit_price");

                    b.HasKey("Id")
                        .HasName("pk_order_items");

                    b.HasIndex("OrderId")
                        .HasDatabaseName("ix_order_items_order_id");

                    b.HasIndex("TicketTypeId")
                        .HasDatabaseName("ix_order_items_ticket_type_id");

                    b.ToTable("order_items", "ticketing");
                });

            modelBuilder.Entity("EventManagement.Ticketing.Domain.Payments.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<decimal?>("AmountRefunded")
                        .HasColumnType("numeric")
                        .HasColumnName("amount_refunded");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("currency");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("order_id");

                    b.Property<DateTime?>("RefundedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("refunded_at");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid")
                        .HasColumnName("transaction_id");

                    b.HasKey("Id")
                        .HasName("pk_payments");

                    b.HasIndex("OrderId")
                        .HasDatabaseName("ix_payments_order_id");

                    b.HasIndex("TransactionId")
                        .IsUnique()
                        .HasDatabaseName("ix_payments_transaction_id");

                    b.ToTable("payments", "ticketing");
                });

            modelBuilder.Entity("EventManagement.Ticketing.Domain.TicketTypes.TicketType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("AvailableQuantity")
                        .HasColumnType("integer")
                        .HasColumnName("available_quantity");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("currency");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid")
                        .HasColumnName("event_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.HasKey("Id")
                        .HasName("pk_ticket_types");

                    b.HasIndex("EventId")
                        .HasDatabaseName("ix_ticket_types_event_id");

                    b.ToTable("ticket_types", "ticketing");
                });

            modelBuilder.Entity("EventManagement.Ticketing.Domain.Tickets.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("Archived")
                        .HasColumnType("boolean")
                        .HasColumnName("archived");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("code");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid")
                        .HasColumnName("customer_id");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid")
                        .HasColumnName("event_id");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("order_id");

                    b.Property<Guid>("TicketTypeId")
                        .HasColumnType("uuid")
                        .HasColumnName("ticket_type_id");

                    b.HasKey("Id")
                        .HasName("pk_tickets");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasDatabaseName("ix_tickets_code");

                    b.HasIndex("CustomerId")
                        .HasDatabaseName("ix_tickets_customer_id");

                    b.HasIndex("EventId")
                        .HasDatabaseName("ix_tickets_event_id");

                    b.HasIndex("OrderId")
                        .HasDatabaseName("ix_tickets_order_id");

                    b.HasIndex("TicketTypeId")
                        .HasDatabaseName("ix_tickets_ticket_type_id");

                    b.ToTable("tickets", "ticketing");
                });

            modelBuilder.Entity("EventManagement.Ticketing.Domain.Orders.Order", b =>
                {
                    b.HasOne("EventManagement.Ticketing.Domain.Customers.Customer", null)
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_orders_customers_customer_id");
                });

            modelBuilder.Entity("EventManagement.Ticketing.Domain.Orders.OrderItem", b =>
                {
                    b.HasOne("EventManagement.Ticketing.Domain.Orders.Order", null)
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_items_orders_order_id");

                    b.HasOne("EventManagement.Ticketing.Domain.TicketTypes.TicketType", null)
                        .WithMany()
                        .HasForeignKey("TicketTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_items_ticket_types_ticket_type_id");
                });

            modelBuilder.Entity("EventManagement.Ticketing.Domain.Payments.Payment", b =>
                {
                    b.HasOne("EventManagement.Ticketing.Domain.Orders.Order", null)
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_payments_orders_order_id");
                });

            modelBuilder.Entity("EventManagement.Ticketing.Domain.TicketTypes.TicketType", b =>
                {
                    b.HasOne("EventManagement.Ticketing.Domain.Events.EventEntity", null)
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_ticket_types_events_event_id");
                });

            modelBuilder.Entity("EventManagement.Ticketing.Domain.Tickets.Ticket", b =>
                {
                    b.HasOne("EventManagement.Ticketing.Domain.Customers.Customer", null)
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_tickets_customers_customer_id");

                    b.HasOne("EventManagement.Ticketing.Domain.Events.EventEntity", null)
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_tickets_events_event_id");

                    b.HasOne("EventManagement.Ticketing.Domain.Orders.Order", null)
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_tickets_orders_order_id");

                    b.HasOne("EventManagement.Ticketing.Domain.TicketTypes.TicketType", null)
                        .WithMany()
                        .HasForeignKey("TicketTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_tickets_ticket_types_ticket_type_id");
                });

            modelBuilder.Entity("EventManagement.Ticketing.Domain.Orders.Order", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}
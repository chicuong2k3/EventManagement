﻿using EventManagement.Ticketing.Domain.Orders;
using EventManagement.Ticketing.Domain.TicketTypes;

namespace EventManagement.Ticketing.Domain.Tickets
{
    public sealed class Ticket : Entity
    {
        private Ticket()
        {
        }

        public Guid Id { get; private set; }

        public Guid CustomerId { get; private set; }

        public Guid OrderId { get; private set; }

        public Guid EventId { get; private set; }

        public Guid TicketTypeId { get; private set; }

        public string Code { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public bool Archived { get; private set; }

        public static Ticket Create(Order order, TicketType ticketType)
        {
            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                CustomerId = order.CustomerId,
                OrderId = order.Id,
                EventId = ticketType.EventId,
                TicketTypeId = ticketType.Id,
                Code = $"tc_{Ulid.NewUlid()}",
                CreatedAt = DateTime.UtcNow
            };

            ticket.Raise(new TicketCreatedDomainEvent(ticket.Id));

            return ticket;
        }

        public void Archive()
        {
            if (Archived)
            {
                return;
            }

            Archived = true;

            Raise(new TicketArchivedDomainEvent(Id, Code));
        }
    }
}

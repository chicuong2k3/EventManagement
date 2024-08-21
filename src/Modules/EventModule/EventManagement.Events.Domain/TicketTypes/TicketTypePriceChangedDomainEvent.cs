﻿namespace EventManagement.Events.Domain.TicketTypes
{
    public sealed class TicketTypePriceChangedDomainEvent(Guid ticketTypeId, decimal price) : DomainEvent
    {
        public Guid TicketId { get; init; } = ticketTypeId;

        public decimal Price { get; init; } = price;
    }
}
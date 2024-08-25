﻿using EventManagement.Common.Application.EventBuses;

namespace EventManagement.Ticketing.IntegrationEvents
{
    public sealed class TicketTypeSoldOutIntegrationEvent : IntegrationEvent
    {
        public TicketTypeSoldOutIntegrationEvent(
            Guid id,
            DateTime occurredOnUtc,
            Guid ticketTypeId)
            : base(id, occurredOnUtc)
        {
            TicketTypeId = ticketTypeId;
        }

        public Guid TicketTypeId { get; init; }
    }
}
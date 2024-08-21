﻿namespace EventManagement.Ticketing.Application.Carts
{
    public sealed class CartItem
    {
        public Guid TicketTypeId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}
namespace EventManagement.Ticketing.Application.UseCases.Carts
{
    public sealed class CartItem
    {
        public Guid TicketId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}
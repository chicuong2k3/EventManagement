namespace EventManagement.Ticketing.Application.Carts
{
    public sealed class Cart
    {
        public Guid CustomerId { get; set; }
        public List<CartItem> CartItems { get; set; } = new();
        internal static Cart CreateDefault(Guid customerId) =>
            new() { CustomerId = customerId };
    }
}

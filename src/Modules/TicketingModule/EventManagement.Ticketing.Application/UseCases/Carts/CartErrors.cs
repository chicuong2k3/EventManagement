namespace EventManagement.Ticketing.Application.UseCases.Carts
{
    public static class CartErrors
    {
        public readonly static Error Empty = Error.Problem("Cart.Empty", "The cart is empty");
    }
}

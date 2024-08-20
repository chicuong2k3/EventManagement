using EventManagement.Common.Domain;

namespace EventManagement.Ticketing.Application.Carts
{
    public static class CartErrors
    {
        public readonly static Error Empty = Error.Problem("Cart.Empty", "The cart is empty");
    }
}

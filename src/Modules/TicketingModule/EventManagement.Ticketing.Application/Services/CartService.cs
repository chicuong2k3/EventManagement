using EventManagement.Common.Application.Caching;
using EventManagement.Ticketing.Application.UseCases.Carts;

namespace EventManagement.Ticketing.Application.Services
{
    public sealed class CartService(ICacheService cacheService)
    {
        public async Task<Cart> GetCartAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            var key = CreateCartKey(customerId);
            var cart = await cacheService.GetAsync<Cart>(key, cancellationToken)
                ?? Cart.CreateDefault(customerId);

            return cart;
        }

        public async Task ClearCartAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            var key = CreateCartKey(customerId);
            var cart = Cart.CreateDefault(customerId);
            await cacheService.SetAsync(key, cart, cancellationToken: cancellationToken);
        }

        public async Task AddItemAsync(Guid customerId, CartItem cartItem, CancellationToken cancellationToken = default)
        {
            var key = CreateCartKey(customerId);
            var cart = await GetCartAsync(customerId, cancellationToken);

            var existingCartItem = cart.CartItems.Find(x => x.TicketTypeId == cartItem.TicketTypeId);

            if (existingCartItem == null)
            {
                cart.CartItems.Add(cartItem);
            }
            else
            {
                existingCartItem.Quantity += cartItem.Quantity;
            }

            await cacheService.SetAsync(key, cart, cancellationToken: cancellationToken);
        }

        public async Task RemoveItemAsync(Guid customerId, Guid ticketId, CancellationToken cancellationToken = default)
        {
            var key = CreateCartKey(customerId);
            var cart = await GetCartAsync(customerId, cancellationToken);

            var existingCartItem = cart.CartItems.Find(x => x.TicketTypeId == ticketId);

            if (existingCartItem == null)
            {
                return;
            }

            cart.CartItems.Remove(existingCartItem);
            await cacheService.SetAsync(key, cart, cancellationToken: cancellationToken);
        }

        static string CreateCartKey(Guid customerId) => $"carts:{customerId}";
    }
}

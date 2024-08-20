using EventManagement.Common.Domain;
using EventManagement.Ticketing.Application.Carts;

namespace EventManagement.Ticketing.Application.Carts.CommandHandlers;

public sealed record GetCartQuery(Guid CustomerId) : IQuery<Cart>;

internal sealed class GetCartQueryHandler(CartService cartService)
    : IQueryHandler<GetCartQuery, Cart>
{
    public async Task<Result<Cart>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        return await cartService.GetCartAsync(request.CustomerId, cancellationToken);
    }
}
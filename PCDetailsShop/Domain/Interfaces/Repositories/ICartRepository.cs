using Domain.Enums;
using Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface ICartRepository
    {
        public Task<(Cart cart, ErrorCodes errorCode)> GetByIdAsync(Guid cartId);

        public Task<Cart> CreateAsync(Cart cart);

        public Task<int> UpdateCartByIdAsync(Guid cartId, Cart newCartData);
    }
}
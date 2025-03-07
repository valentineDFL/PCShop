using Domain.Enums;
using Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface ICartRepository
    {
        public Task<(Cart cart, ErrorCodes errorCode)> GetByIdAsync(Guid cartId);

        public Task<Cart> CreateAsync(Cart cart);

        public Task<(Product product, ErrorCodes errorCode)> AddProductAsync(Guid cartId, Guid productId);

        public Task<(Guid productId, ErrorCodes errorCode)> RemoveProductAsync(Guid cartId, Guid productId);

        public Task<(int removedProducts, ErrorCodes errorCode)> BuyProductsFromCartAsync(Guid cartId);
    }
}
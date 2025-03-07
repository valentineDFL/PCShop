using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.VisualBasic;

namespace DataAccessLayer.Repositories
{
    internal class CartRepository : ICartRepository
    {
        private readonly PcShopDbContext _dbContext;

        public CartRepository(PcShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(Cart cart, ErrorCodes errorCode)> GetByIdAsync(Guid id)
        {
            Cart cart = await _dbContext.Carts
                .AsNoTracking()
                .Include(c => c.Products)
                .ThenInclude(p => p.CharacteristicsRealizations)
                .ThenInclude(cr => cr.CharacteristicPattern)
                .Include(c => c.User)
                .Include(c => c.Products)
                .ThenInclude(p => p.Categories)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (cart == null)
                return (null, ErrorCodes.CartNotFound);

            return (cart, ErrorCodes.None);
        }

        public async Task<Cart> CreateAsync(Cart cart)
        {
            await _dbContext.Carts.AddAsync(cart);
            await _dbContext.SaveChangesAsync();

            return cart;
        }

        public async Task<(Product product, ErrorCodes errorCode)> AddProductAsync(Guid cartId, Guid productId)
        {
            Cart findedCart = await _dbContext.Carts
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (findedCart == null)
                return (null, ErrorCodes.CartNotFound);

            Product findedProduct = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (findedCart == null)
                return (null, ErrorCodes.ProductNotFound);

            findedCart.Products.Add(findedProduct);
            findedCart.CartTotalPrice += findedProduct.Price;
            findedCart.CartTotalWeight += findedProduct.Weight;

            await _dbContext.SaveChangesAsync();

            return (findedProduct, ErrorCodes.None);
        }

        public async Task<(Guid productId, ErrorCodes errorCode)> RemoveProductAsync(Guid cartId, Guid productId)
        {
            Cart findedCart = await _dbContext.Carts
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (findedCart == null)
                return (Guid.Empty, ErrorCodes.CartNotFound);

            Product findedProduct = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (findedCart == null)
                return (Guid.Empty, ErrorCodes.ProductNotFound);

            findedCart.Products.Remove(findedProduct);
            findedCart.CartTotalPrice -= findedProduct.Price;
            findedCart.CartTotalWeight -= findedProduct.Weight;

            await _dbContext.SaveChangesAsync();

            return (findedProduct.Id, ErrorCodes.None);
        }
        
        public async Task<(int removedProducts, ErrorCodes errorCode)> BuyProductsFromCartAsync(Guid cartId)
        {
            Cart findedCart = await _dbContext.Carts
                .Include(c => c.User)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (findedCart == null)
                return (0, ErrorCodes.CartNotFound);

            if (findedCart.User.WalletBalance < findedCart.CartTotalPrice)
                return (0, ErrorCodes.InsufficientFunds);

            int productsToRemoveCount = findedCart.Products.Count;

            findedCart.Products.Clear();
            findedCart.CartTotalPrice = 0;
            findedCart.CartTotalWeight = 0;

            await _dbContext.SaveChangesAsync();

            return (productsToRemoveCount, ErrorCodes.None);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using DataAccessLayer.Mapping;
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
        private readonly CartMapper _cartMapper;

        public CartRepository(PcShopDbContext dbContext, CartMapper cartMapper)
        {
            _dbContext = dbContext;
            _cartMapper = cartMapper;
        }

        public async Task<(Cart cart, ErrorCodes errorCode)> GetByIdAsync(Guid id)
        {
            CartEntity cart = await _dbContext.Carts
                .Include(x => x.Products)
                .ThenInclude(x => x.Categories)
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (cart == null)
                return (null, ErrorCodes.CartNotFound);

            Cart result = await _cartMapper.EntityToModelAsync(cart);

            return (result, ErrorCodes.None);
        }

        public async Task<Cart> CreateAsync(Cart cart)
        {
            CartEntity entity = await _cartMapper.ModelToEntityAsync(cart);

            await _dbContext.Carts.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return cart;
        }

        public async Task<int> UpdateCartByIdAsync(Guid cartId, Cart newCartData)
        {
            CartEntity mappedCart = await _cartMapper.ModelToEntityAsync(newCartData);

            int updatedCarts = await _dbContext.Carts
                .Where(c => c.Id == cartId)
                .ExecuteUpdateAsync(c => c
                .SetProperty(c => c.Products, mappedCart.Products)
                .SetProperty(c => c.CartTotalPrice, mappedCart.CartTotalPrice)
                .SetProperty(c => c.CartTotalWeight, mappedCart.CartTotalWeight));

            await _dbContext.SaveChangesAsync();

            return updatedCarts;
        }
    }
}
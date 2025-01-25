using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using DataAccessLayer.Mapping;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccessLayer.Repositories
{
    internal class CartRepository //: IRepository<Cart>
    {
        private readonly PcShopDbContext _dbContext;
        private readonly CartMapper _cartMapper;

        public CartRepository(PcShopDbContext dbContext, CartMapper cartMapper)
        {
            _dbContext = dbContext;
            _cartMapper = cartMapper;
        }

        public async Task<List<Cart>> GetAllAsync()
        {
            List<CartEntity> entities = await _dbContext.Carts.ToListAsync();
            List<Cart> carts = await _cartMapper.EntitiesToModelsAsync(entities);
            
            return carts;
        }

        public async Task<Cart> CreateAsync(Cart cart)
        {
            if(cart == null)
                throw new ArgumentNullException($"Cart null {nameof(CreateAsync)}");

            CartEntity entity = await _cartMapper.ModelToEntityAsync(cart);

            await _dbContext.Carts.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return cart;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            int deletedCarts = await _dbContext.Carts
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();

            if (deletedCarts == 0)
                throw new ArgumentException("Cart not found");

            return id;
        }

        public async Task<Cart> UpdateAsync(Cart cart)
        {
            if (cart == null)
                throw new ArgumentNullException($"Cart null {nameof(UpdateAsync)}");

            CartEntity entity = await _cartMapper.ModelToEntityAsync(cart);

            await _dbContext.Carts
                .Where(c => c.Id == entity.Id)
                .ExecuteUpdateAsync(c => c
                .SetProperty(p => p.CartTotalPrice, entity.CartTotalPrice)
                .SetProperty(p => p.CartTotalWeight, entity.CartTotalWeight)
                .SetProperty(p => p.Products, entity.Products));

            await _dbContext.SaveChangesAsync();

            return cart;
        }
    }
}
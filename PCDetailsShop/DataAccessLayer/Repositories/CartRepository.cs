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

namespace DataAccessLayer.Repositories
{
    internal class CartRepository : IRepository<Cart>
    {
        private readonly PcShopDbContext _dbContext;
        private readonly CartMapper _cartMapper;

        public CartRepository(PcShopDbContext dbContext, CartMapper cartMapper)
        {
            _dbContext = dbContext;
            _cartMapper = cartMapper;
        }

        public async Task<CollectionResult<Cart>> GetAllAsync()
        {
            List<CartEntity> entities = await _dbContext.Carts
                .AsNoTracking()
                .Include(x => x.User)
                .ToListAsync();

            if(entities.Count == 0)
            {
                return new CollectionResult<Cart>()
                {
                    Count = 0,
                    ErrorCode = (int)ErrorCodes.CartsNotFound,
                    ErrorMessage = ErrorCodes.CartsNotFound.ToString()
                };
            }
            
            List<Cart> carts = await _cartMapper.EntitiesToModelsAsync(entities);
            
            return new CollectionResult<Cart>()
            {
                Count = carts.Count,
                Data = carts,
            };
        }

        public async Task<BaseResult<Cart>> GetByIdAsync(Guid id)
        {
            CartEntity cart = await _dbContext.Carts
                .Include(x => x.Products)
                .ThenInclude(x => x.Categories)
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if(cart == null)
            {
                return new BaseResult<Cart>()
                {
                    ErrorCode = (int)ErrorCodes.CartNotFound,
                    ErrorMessage = ErrorCodes.CartNotFound.ToString()
                };
            }

            Cart result = await _cartMapper.EntityToModelAsync(cart);

            return new BaseResult<Cart>() { Data = result };
        }

        public async Task<BaseResult<Cart>> CreateAsync(Cart cart)
        {
            if (cart == null)
                throw new ArgumentNullException($"Cart null {nameof(CreateAsync)}");

            CartEntity entity = await _cartMapper.ModelToEntityAsync(cart);

            await _dbContext.Carts.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return new BaseResult<Cart>() { Data = cart };
        }

        public async Task<BaseResult<Guid>> DeleteAsync(Guid id)
        {
            if(id == Guid.Empty)
                throw new ArgumentException($"Cart id is Empty {nameof(DeleteAsync)}");

            int countDeletedCarts = await _dbContext.Carts
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();

            if (countDeletedCarts == 0)
            {
                return new BaseResult<Guid> 
                {
                    ErrorCode = (int)ErrorCodes.CartNotFound,
                    ErrorMessage = ErrorCodes.CartNotFound.ToString()
                };
            }

            await _dbContext.SaveChangesAsync();

            return new BaseResult<Guid>() { Data = id };
        }

        public async Task<BaseResult<Cart>> UpdateAsync(Cart cart)
        {
            if (cart == null)
                throw new ArgumentNullException($"Cart null {nameof(UpdateAsync)}");

            CartEntity entity = await _cartMapper.ModelToEntityAsync(cart);

            int updatedCartsCount = await _dbContext.Carts
                .Where(c => c.Id == entity.Id)
                .ExecuteUpdateAsync(c => c
                .SetProperty(p => p.CartTotalPrice, entity.CartTotalPrice)
                .SetProperty(p => p.CartTotalWeight, entity.CartTotalWeight)
                .SetProperty(p => p.Products, entity.Products));

            if(updatedCartsCount == 0)
            {
                return new BaseResult<Cart>()
                {
                    ErrorCode = (int)ErrorCodes.CartNotFound,
                    ErrorMessage = ErrorCodes.CartNotFound.ToString()
                };
            }

            await _dbContext.SaveChangesAsync();

            return new BaseResult<Cart>() { Data = cart };
        }
    }
}
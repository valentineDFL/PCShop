using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using Domain.Models;

namespace DataAccessLayer.Mapping
{
    internal class CartMapper
    {
        private readonly UserMapper _userMapper;
        private readonly ProductMapper _productMapper;

        public CartMapper(UserMapper userMapper, ProductMapper productMapper)
        {
            _userMapper = userMapper;
            _productMapper = productMapper;
        }

        public async Task<CartEntity> ModelToEntityAsync(Cart cart)
        {
            if (cart == null)
                throw new ArgumentNullException($"Cart is null {nameof(ModelToEntityAsync)}");

            return new CartEntity()
            {
                Id = cart.Id,
                CartTotalPrice = cart.CartTotalPrice,
                CartTotalWeight = cart.CartTotalWeight,
                User = null,
                UserId = cart.UserId,
                Products = await _productMapper.ModelsToEntitiesAsync((List<Product>)cart.Products),
            };
        }

        public async Task<Cart> EntityToModelAsync(CartEntity cartEntity)
        {
            if (cartEntity == null)
                throw new ArgumentNullException($"Cart entity is null {nameof(EntityToModelAsync)}");

            return new Cart
                    (
                        cartEntity.Id,
                        cartEntity.CartTotalPrice,
                        cartEntity.CartTotalWeight,
                        _userMapper.EntityToModel(cartEntity.User),
                        cartEntity.UserId,
                        await _productMapper.EntitiesToModelsAsync(cartEntity.Products)
                    );
        }
    }
}
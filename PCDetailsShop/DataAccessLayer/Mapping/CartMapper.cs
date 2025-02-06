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
            CartEntity cartEntity = new CartEntity()
            {
                Id = cart.Id,
                CartTotalPrice = cart.CartTotalPrice,
                CartTotalWeight = cart.CartTotalWeight,
                User = null,
                UserId = cart.UserId,
                Products = await _productMapper.ModelsToEntitiesAsync((List<Product>)cart.Products),
            };

            return cartEntity;
        }

        public async Task<Cart> EntityToModelAsync(CartEntity cartEntity)
        {
            Cart cart = new Cart
                    (
                        id: cartEntity.Id,
                        cartTotalPrice: cartEntity.CartTotalPrice,
                        cartTotalWeight: cartEntity.CartTotalWeight,
                        user: _userMapper.EntityToModel(cartEntity.User),
                        userId: cartEntity.UserId,
                        products: await _productMapper.EntitiesToModelsAsync(cartEntity.Products)
                    );

            return cart;
        }

        public async Task<List<Cart>> EntitiesToModelsAsync(List<CartEntity> entities)
        {
            List<Cart> carts = new List<Cart>();

            foreach (CartEntity entity in entities)
            {
                carts.Add(await EntityToModelAsync(entity));
            }

            return carts;
        }
    }
}
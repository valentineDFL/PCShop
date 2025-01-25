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
        private readonly ProductMapper _productMapper;

        public CartMapper(ProductMapper productMapper)
        {
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

        public Cart EntityToModel(CartEntity cartEntity)
        {
            Console.WriteLine(cartEntity == null);

            Cart cart = new Cart
                    (
                        id: cartEntity.Id,
                        cartTotalPrice: cartEntity.CartTotalPrice,
                        cartTotalWeight: cartEntity.CartTotalWeight,
                        user: null,
                        userId: cartEntity.UserId,
                        products: null
                    );

            return cart;
        }

        public async Task<List<CartEntity>> ModelsToEntitiesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Cart>> EntitiesToModelsAsync(List<CartEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
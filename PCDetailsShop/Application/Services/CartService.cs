using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Result;
using Microsoft.AspNetCore.Mvc.Formatters;
using Serilog;

namespace Application.Services
{
    internal class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        private readonly ILogger _logger;

        public CartService(ICartRepository cartRepository, ILogger logger)
        {
            _cartRepository = cartRepository;
            _logger = logger;
        }

        public async Task<CollectionResult<Product>> GetAllProductsInCartAsync(Guid cartId)
        {
            (Cart Cart, ErrorCodes errorCode) cart = await _cartRepository.GetByIdAsync(cartId);

            if (cart.errorCode != ErrorCodes.None)
            {
                return new CollectionResult<Product>()
                {
                    ErrorCode = (int)cart.errorCode,
                    ErrorMessage = cart.errorCode.ToString(),
                };
            }

            return new CollectionResult<Product>() { Data = (List<Product>)cart.Cart.Products, Count = cart.Cart.Products.Count };
        }

        public async Task<BaseResult<Product>> GetProductFromCartByNameAsync(Guid cartId, string productName)
        {
            (Cart cart, ErrorCodes errorCode) findedCart = await _cartRepository.GetByIdAsync(cartId);

            if(findedCart.errorCode != ErrorCodes.None)
            {
                return new BaseResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.CartNotFound,
                    ErrorMessage = ErrorCodes.CartNotFound.ToString()
                };
            }

            Product findedProduct = findedCart.cart.Products.FirstOrDefault(x => x.Name == productName);

            if (findedProduct == null)
            {
                return new BaseResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorCodes.ProductNotFound.ToString()
                };
            }

            return new BaseResult<Product>() { Data = findedProduct };
        }

        public async Task<BaseResult<decimal>> GetProductsTotalPriceByIdAsync(Guid cartId)
        {
            (Cart Cart, ErrorCodes errorCode) cart = await _cartRepository.GetByIdAsync(cartId);

            if (cart.errorCode != ErrorCodes.None)
            {
                return new BaseResult<decimal>()
                {
                    ErrorCode = (int)cart.errorCode,
                    ErrorMessage = cart.errorCode.ToString(),
                };
            }

            return new BaseResult<decimal>() { Data = cart.Cart.CartTotalPrice };
        }

        public async Task<BaseResult<float>> GetProductsTotalWeightAsync(Guid cartId)
        {
            (Cart cart, ErrorCodes errorCode) cart = await _cartRepository.GetByIdAsync(cartId);

            if (cart.errorCode != ErrorCodes.None)
            {
                return new BaseResult<float>()
                {
                    ErrorCode = (int)cart.errorCode,
                    ErrorMessage = cart.errorCode.ToString(),
                };
            }

            return new BaseResult<float>() { Data = cart.cart.CartTotalWeight };
        }

        public async Task<BaseResult<int>> BuyProductsAsync(Guid cartId)
        {
            var buyedProducts = await _cartRepository.BuyProductsFromCartAsync(cartId);

            if(buyedProducts.errorCode != ErrorCodes.None)
            {
                return new BaseResult<int>()
                {
                    ErrorCode = (int)buyedProducts.errorCode,
                    ErrorMessage = buyedProducts.errorCode.ToString()
                };
            }

            return new BaseResult<int>() { Data = buyedProducts.removedProducts };
        }

        public async Task<BaseResult<Guid>> RemoveProductFromCartByIdAsync(Guid cartId, Guid productId)
        {
            var removedProduct = await _cartRepository.RemoveProductAsync(cartId, productId);

            if(removedProduct.errorCode != ErrorCodes.None)
            {
                return new BaseResult<Guid>()
                {
                    ErrorCode = (int)removedProduct.errorCode,
                    ErrorMessage = removedProduct.errorCode.ToString(),
                };
            }

            return new BaseResult<Guid>() { Data = removedProduct.productId };
        }
    }
}

using System;
using System.Collections.Generic;
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

            return new CollectionResult<Product>() { Data = cart.Cart.Products, Count = cart.Cart.Products.Count };
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

        public async Task<CollectionResult<Product>> BuyProductsAsync(Guid cartId) // paymants service
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

            if (cart.Cart.Products.Count == 0)
            {
                _logger.Warning(ErrorCodes.ProductsNotFound.ToString());

                return new CollectionResult<Product>()
                {
                    Count = 0,
                    ErrorCode = (int)ErrorCodes.ProductsNotFound,
                    ErrorMessage = ErrorCodes.ProductsNotFound.ToString(),
                };
            }

            if(cart.Cart.CartTotalPrice > cart.Cart.User.WalletBalance)
            {
                return new CollectionResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.NotEnoughFundsInTheUsersBalance,
                    ErrorMessage = ErrorCodes.NotEnoughFundsInTheUsersBalance.ToString()
                };
            }

            Cart updatedCart = new Cart(cart.Cart.Id, 0, 0, cart.Cart.User, cart.Cart.UserId, new List<Product>());

            await _cartRepository.UpdateCartByIdAsync(cartId, updatedCart);

            return new CollectionResult<Product>() { Count = cart.Cart.Products.Count, Data = cart.Cart.Products };
        }

        public async Task<BaseResult<Product>> RemoveProductFromCartByIdAsync(Guid cartId, Guid productId)
        {
            (Cart Cart, ErrorCodes errorCode) cart = await _cartRepository.GetByIdAsync(cartId);

            if (cart.errorCode != ErrorCodes.None)
            {
                return new BaseResult<Product>()
                {
                    ErrorCode = (int)cart.errorCode,
                    ErrorMessage = cart.errorCode.ToString(),
                };
            }

            Product productToDelete = cart.Cart.Products.FirstOrDefault(x => x.Id == productId);

            if(productToDelete == null)
            {
                return new BaseResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.CartDoesNotContainSelectedProduct,
                    ErrorMessage = ErrorCodes.CartDoesNotContainSelectedProduct.ToString(),
                };
            }

            decimal newCartTotalPrice = cart.Cart.CartTotalPrice - productToDelete.Price;
            float newCartTotalWeight = cart.Cart.CartTotalWeight - productToDelete.Weight;

            List<Product> productsWithoutDeletedProduct = cart.Cart.Products.ToList();
            productsWithoutDeletedProduct.Remove(productToDelete);

            Cart updatedCart = new Cart(cart.Cart.Id, newCartTotalPrice, newCartTotalWeight, cart.Cart.User, cart.Cart.UserId, productsWithoutDeletedProduct);

            await _cartRepository.UpdateCartByIdAsync(cartId, updatedCart);

            return new BaseResult<Product>() { Data = productToDelete };
        }
    }
}

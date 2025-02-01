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
        private readonly IRepository<Cart> _cartRepository;

        private readonly ILogger _logger;

        public CartService(IRepository<Cart> cartRepository, ILogger logger)
        {
            _cartRepository = cartRepository;
            _logger = logger;
        }

        public async Task<CollectionResult<Product>> GetAllProductsInCartAsync(Guid cartId)
        {
            try
            {
                BaseResult<Cart> cart = await _cartRepository.GetByIdAsync(cartId);

                if (!cart.IsSuccess)
                {
                    return new CollectionResult<Product>()
                    {
                        ErrorCode = cart.ErrorCode,
                        ErrorMessage = cart.ErrorMessage,
                    };
                }

                return new CollectionResult<Product>()
                {
                    Data = cart.Data.Products,
                    Count = cart.Data.Products.Count
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new CollectionResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString()
                };
            }
        }

        public async Task<BaseResult<decimal>> GetProductsSummCost(Guid cartId)
        {
            try
            {
                BaseResult<Cart> cart = await _cartRepository.GetByIdAsync(cartId);

                if (!cart.IsSuccess)
                {
                    return new BaseResult<decimal>()
                    {
                        ErrorCode = cart.ErrorCode,
                        ErrorMessage = cart.ErrorMessage,
                    };
                }

                return new BaseResult<decimal>()
                {
                    Data = cart.Data.Products.Sum(x => x.Price)
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<decimal>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString()
                };
            }
        }

        public async Task<BaseResult<float>> GetProductsSummWeight(Guid cartId)
        {
            try
            {
                BaseResult<Cart> cart = await _cartRepository.GetByIdAsync(cartId);

                if (!cart.IsSuccess)
                {
                    return new BaseResult<float>()
                    {
                        ErrorCode = cart.ErrorCode,
                        ErrorMessage = cart.ErrorMessage,
                    };
                }

                return new BaseResult<float>()
                {
                    Data = cart.Data.Products.Sum(x => x.Weight)
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<float>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString()
                };
            }
        }

        public async Task<CollectionResult<Product>> BuyProductsAsync(Guid cartId) // paymants service
        {
            try
            {
                BaseResult<Cart> cart = await _cartRepository.GetByIdAsync(cartId);

                if (!cart.IsSuccess)
                {
                    return new CollectionResult<Product>()
                    {
                        ErrorCode = cart.ErrorCode,
                        ErrorMessage = cart.ErrorMessage,
                    };
                }

                decimal summCost = cart.Data.Products.Sum(x => x.Price);

                if(cart.Data.User.WalletBalance < summCost)
                {
                    return new CollectionResult<Product>()
                    {
                        ErrorCode = (int)ErrorCodes.NotEnoughFundsInTheUsersBalance,
                        ErrorMessage = ErrorCodes.NotEnoughFundsInTheUsersBalance.ToString()
                    };
                }

                Cart oldCart = cart.Data;
                Cart updatedCart = new Cart(oldCart.Id, 0, 0, oldCart.User, oldCart.UserId, new List<Product>());
                

            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new CollectionResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString()
                };
            }
            throw new NotImplementedException();
        }

        public async Task<BaseResult<Product>> RemoveProductFromCartByIdAsync(Guid cartId, Guid productId)
        {
            try
            {
                BaseResult<Cart> cart = await _cartRepository.GetByIdAsync(cartId);

                if (!cart.IsSuccess)
                {
                    return new BaseResult<Product>()
                    {
                        ErrorCode = cart.ErrorCode,
                        ErrorMessage = cart.ErrorMessage,
                    };
                }

                List<Product> products = cart.Data.Products.ToList();

                Product product = products.FirstOrDefault(p => p.Id == productId);

                bool removeResult = products.Remove(product);

                if (!removeResult)
                {
                    return new BaseResult<Product>()
                    {
                        ErrorCode = (int)ErrorCodes.ProductNotFound,
                        ErrorMessage = ErrorCodes.ProductNotFound.ToString()
                    };
                }

                Cart oldCart = cart.Data;
                Cart updatedCart = new Cart(oldCart.Id, oldCart.CartTotalPrice - product.Price, oldCart.CartTotalWeight - product.Weight, oldCart.User, oldCart.UserId, products);

                BaseResult<Cart> updateResult = await _cartRepository.UpdateAsync(updatedCart);

                if (!updateResult.IsSuccess)
                {
                    return new BaseResult<Product>()
                    {
                        ErrorCode = updateResult.ErrorCode,
                        ErrorMessage = updateResult.ErrorMessage,
                    };
                }

                return new BaseResult<Product>()
                {
                    Data = product,
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString()
                };
            }
        }
    }
}

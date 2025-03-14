﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Services
{
    public interface ICartService
    {
        public Task<CollectionResult<Product>> GetAllProductsInCartAsync(Guid cartId);

        public Task<BaseResult<Product>> GetProductFromCartByNameAsync(Guid cartId, string productName);

        public Task<BaseResult<Guid>> RemoveProductFromCartByIdAsync(Guid cartId, Guid productId);

        public Task<BaseResult<int>> BuyProductsAsync(Guid cartId);

        public Task<BaseResult<decimal>> GetProductsTotalPriceByIdAsync(Guid cartId);

        public Task<BaseResult<float>> GetProductsTotalWeightAsync(Guid cartId);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Services
{
    internal interface ICartService
    {
        public Task<CollectionResult<Product>> GetAllProductsInCartAsync();

        public Task<BaseResult<Product>> RemoveProductFromCartByIdAsync(Guid productId);

        public Task<CollectionResult<Product>> BuyProductsAsync();

        public Task<BaseResult<decimal>> GetProductsSummCost();

        public Task<BaseResult<float>> GetProductsSummWeight();
    }
}

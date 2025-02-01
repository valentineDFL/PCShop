using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.CategoryDtos;
using Domain.Dto.ProductDtos;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Services
{
    public interface IProductService
    {
        public Task<CollectionResult<Product>> GetAllAsync();

        public Task<BaseResult<Product>> GetByIdAsync(Guid id);

        public Task<CollectionResult<Product>> GetByNamePartAsync(string namePart);

        public Task<BaseResult<Product>> GetByCategoryNameAsync(string categoryName);

        public Task<BaseResult<Product>> CreateAsync(CreateProductDto dto);

        public Task<BaseResult<Product>> DeleteByIdAsync(Guid id);

        public Task<BaseResult<Product>> UpdateByIdAsync(UpdateProductDto dto);

        public Task<BaseResult<Guid>> AddCategoryAsync(string categoryName);

        public Task<BaseResult<Guid>> RemoveCategoryFromProductAsync(string categoryName);

        public Task<BaseResult<Product>> AddProductToCartAsync(Guid productId, Guid userId);
    } 
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.CategoryDtos;
using Domain.Dto.ProductDtos;
using Domain.Result;

namespace Domain.Interfaces.Services
{
    public interface IProductService
    {
        public Task<BaseResult<ProductDto>> GetAllProductsAsync();

        public Task<BaseResult<ProductDto>> GetProductByIdAsync(Guid id);

        public Task<ResultCollection<ProductDto>> GetProductsByNamePartAsync(string name);

        public Task<BaseResult<ProductDto>> GetProductsByCategoryIdAsync(Guid id);

        public Task<BaseResult<ProductDto>> GetProductsByCategoryAsync(string name);

        public Task<BaseResult<ProductDto>> CreateProductAsync(CreateProductDto dto);

        public Task<BaseResult<ProductDto>> DeleteProductByIdAsync(Guid id);

        public Task<BaseResult<ProductDto>> UpdateProductByIdAsync(Guid id);

        public Task<BaseResult<CategoryDto>> AddCategoryToProductAsync(CategoryDto category);

        public Task<BaseResult<CategoryDto>> RemoveCategoryFromProductAsync(CategoryDto category);

        public Task<BaseResult<ProductDto>> AddProductToCartAsync(Guid productId, Guid userId);
    } 
}

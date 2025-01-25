using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.CategoryDtos;
using Domain.Dto.ProductDtos;
using Domain.Result;

namespace Domain.Interfaces.Services
{
    public interface ICategoryService
    {
        public Task<BaseResult<CategoryDto>> GetAllCategoriesAsync();

        public Task<BaseResult<CategoryDto>> GetCategoryByNamePartAsync(string namePart);

        public Task<ResultCollection<ProductDto>> GetAllCategoryProducts(string namePart);

        public Task<BaseResult<CategoryDto>> CreateCategoryAsync(CreateCategoryDto dto);

        public Task<BaseResult<CategoryDto>> DeleteCategoryByIdAsync(Guid id);

        public Task<BaseResult<CategoryDto>> UpdateCategoryByIdAsync(Guid id);

        public Task<BaseResult<CategoryDto>> AddProductToCategoryAsync(string categoryName, Guid productId);

        public Task<BaseResult<CategoryDto>> DeleteProductFromCategoryAsync(string categoryName, Guid productId);
    }
}

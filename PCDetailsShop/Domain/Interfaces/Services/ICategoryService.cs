using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.CategoryDtos;
using Domain.Dto.ProductDtos;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Services
{
    public interface ICategoryService
    {
        public Task<CollectionResult<Category>> GetAllAsync();

        public Task<BaseResult<Category>> GetByIdAsync(Guid categoryId);

        public Task<BaseResult<Category>> GetByNamePartAsync(string namePart);

        public Task<CollectionResult<Product>> GetAllCategoryProducts(string categoryNamePart);

        public Task<BaseResult<Category>> CreateAsync(CreateCategoryDto dto);

        public Task<BaseResult<Guid>> DeleteByIdAsync(Guid categoryId);

        public Task<BaseResult<Category>> ChangeNameAsync(Guid categoryId, string newName);

        public Task<BaseResult<Product>> AddProductToCategoryAsync(string categoryName, Guid productId);

        public Task<BaseResult<Product>> DeleteProductFromCategoryAsync(string categoryName, Guid productId);
    }
}
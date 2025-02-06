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

        public Task<CollectionResult<Category>> GetProductCategoriesByNameAsync(string categoryName);

        public Task<BaseResult<Product>> CreateAsync(CreateProductDto dto);

        public Task<BaseResult<Product>> DeleteByIdAsync(Guid id);

        public Task<BaseResult<string>> ChangeNameByIdAsync(Guid productId);

        public Task<BaseResult<string>> ChangeDescriptionByIdAsync(Guid productId);

        public Task<BaseResult<decimal>> ChangePriceByIdAsync(Guid productId);

        public Task<BaseResult<float>> ChangeWeightByIdAsync(Guid productId);

        public Task<BaseResult<Category>> AddCategoryToProductByIdAsync(Guid productId, Guid categoryId);

        public Task<BaseResult<Category>> DeleteCategoryFromProductByIdAsync(Guid productId, Guid categoryId);

        public Task<BaseResult<Product>> AddProductToCartAsync(Guid productId, Guid cartId);
    } 
}
using Domain.Dto.CategoryDtos;
using Domain.Dto.ProductDtos;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Services
{
    public interface ICategoryService // Unit Of Work в будущем
    {
        public Task<CollectionResult<Category>> GetAllAsync();

        public Task<BaseResult<Category>> GetByIdAsync(Guid categoryId);

        public Task<CollectionResult<Category>> GetByNamePartAsync(string namePart);

        public Task<BaseResult<Category>> GetByNameAsync(string name);

        public Task<CollectionResult<Product>> GetAllCategoryProductsById(Guid categoryId);

        public Task<CollectionResult<Product>> GetAllCategoryProductsByName(string name);

        public Task<BaseResult<Category>> CreateAsync(CreateCategoryDto dto);

        public Task<BaseResult<Guid>> DeleteByIdAsync(Guid categoryId);

        public Task<BaseResult<Category>> ChangeNameAsync(Guid categoryId, string newName);


        public Task<CollectionResult<Product>> AddProductsToCategoryAsync(Guid categoryId, List<Guid> productsId);

        public Task<CollectionResult<Product>> DeleteProductsFromCategoryAsync(Guid categoryId, List<Guid> productsId);

        public Task<CollectionResult<CharacteristicPattern>> GetCharacteristicsByCategoryIdAsync(Guid categoryId);

        public Task<BaseResult<CharacteristicPattern>> ChangeCharacteristicNameByNameAsync(Guid categoryId, string characteristicPatternName, string newCharacteristicPatternName);

        public Task<CharacteristicPattern> AddCharacteristicsToCategoryAsync(Guid categoryId, List<Guid> characteristicsId);

        public Task<CharacteristicPattern> DeleteCharacteristicsFromCategoryAsync(Guid categoryId, List<Guid> characteristicsId);
    }
}
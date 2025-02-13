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

        public Task<CollectionResult<Category>> GetByNameAsync(string name);

        public Task<BaseResult<Category>> CreateAsync(CreateCategoryDto dto);

        public Task<BaseResult<Guid>> DeleteByIdAsync(Guid categoryId);

        public Task<BaseResult<string>> ChangeNameByIdAsync(Guid categoryId, string newName);

        public Task<CollectionResult<CharacteristicPattern>> GetCharacteristicsByCategoryIdAsync(Guid categoryId);

        public Task<BaseResult<string>> ChangeCharacteristicNameByNameAsync(Guid categoryId, string characteristicPatternName, string newCharacteristicPatternName);
    }
}
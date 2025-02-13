using Domain.Models;

namespace Domain.Interfaces.Repositories
{
	public interface ICharacteristicPatternRepository
	{
		public Task<CharacteristicPattern> GetByIdAsync(Guid id);

		public Task<CharacteristicPattern> GetByNameAsync(string name);

		public Task<List<CharacteristicPattern>> CreateAsync(List<CharacteristicPattern> patternsToCreate);

		public Task<int> ChangeNameAsync(Guid patternId, string newName);

		public Task<int> DeletePatternById(Guid patternId);
		
		public Task<int> DeleteCategoryPatternsByCategoryIdAsync(Guid categoryId);
	}
}
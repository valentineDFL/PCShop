using Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface ICharacteristicPatternRepository
    {
        public Task<CharacteristicPattern> GetByIdAsync(Guid id);

        public Task<string> ChangePatternNameById(Guid categoryId, string newName);

        public Task<int> DeletePatternById(Guid patternId);
    }
}
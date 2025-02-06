using Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface ICharacteristicRealizeRepository
    {
        public Task<CharacteristicRealization> GetByIdAsync(Guid id);

        public Task<string> ChangeRealizationValueById(Guid id, string newValue);

        public Task<int> DeletePatternById(Guid patternId);
    }
}
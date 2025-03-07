using Domain.Enums;
using Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface ICharacteristicRealizationRepository
    {
        public Task<int> ChangeRealizationValueByIdAsync(Guid id, string newValue);

        public Task<int> DeleteRealizationsAsync(List<CharacteristicRealization> realizations);

        public Task<List<CharacteristicRealization>> CreateRangeAsync(List<CharacteristicRealization> realizations);
    }
}
using System;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    internal class CharacteristicRealizationRepository : ICharacteristicRealizationRepository
    {
        private readonly PcShopDbContext _dbContext;

        public CharacteristicRealizationRepository(PcShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> ChangeRealizationValueByIdAsync(Guid id, string newValue)
        {
            int updatedRealizations = await _dbContext.CharacteristicRealizations
                .Where(cr => cr.Id == id)
                .ExecuteUpdateAsync(cr => cr
                .SetProperty(cr => cr.Value, newValue));

            await _dbContext.SaveChangesAsync();

            return updatedRealizations;
        }

        public async Task<int> DeleteRealizationsAsync(List<CharacteristicRealization> realizations)
        {
            int deletedRealizations = await _dbContext.CharacteristicRealizations
                .Where(cr => realizations.Any(rl => rl.Id == cr.Id))
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();

            return deletedRealizations;
        }

        public async Task<List<CharacteristicRealization>> CreateRangeAsync(List<CharacteristicRealization> realizations)
        {
            await _dbContext.CharacteristicRealizations.AddRangeAsync(realizations);

            await _dbContext.SaveChangesAsync();

            return realizations;
        }
    }
}
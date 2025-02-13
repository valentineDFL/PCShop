using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DataAccessLayer.Entities;
using DataAccessLayer.Entities.Characteristic;
using DataAccessLayer.Mapping;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
	class CharacteristicPatternRepository : ICharacteristicPatternRepository
	{
		private readonly PcShopDbContext _dbContext;
		private readonly CharacteristicPatternMapper _characteristicPatternMapper;

		public CharacteristicPatternRepository(PcShopDbContext dbContext, CharacteristicPatternMapper characteristicPatternMapper)
		{
			_dbContext = dbContext;
			_characteristicPatternMapper = characteristicPatternMapper;
		}

		public async Task<CharacteristicPattern> GetByIdAsync(Guid id)
		{
			CharacteristicPatternEntity characteristicPatternEntity = await _dbContext.CharacteristicPatterns
				.FirstOrDefaultAsync(c => c.Id == id);

			if (characteristicPatternEntity == null)
				return new CharacteristicPattern();

			return _characteristicPatternMapper.EntityToModel(characteristicPatternEntity);
		}

		public async Task<CharacteristicPattern> GetByNameAsync(string name)
		{
			CharacteristicPatternEntity characteristicPatternEntity = await _dbContext.CharacteristicPatterns
				.FirstOrDefaultAsync(c => c.Name == name);

			if (characteristicPatternEntity == null)
				return new CharacteristicPattern();

			return _characteristicPatternMapper.EntityToModel(characteristicPatternEntity);
		}

		public async Task<List<CharacteristicPattern>> CreateAsync(List<CharacteristicPattern> patternsToCreate)
		{
			List<CharacteristicPatternEntity> patternsToCreateEntities = _characteristicPatternMapper.ModelsToEntities(patternsToCreate);

			await _dbContext.CharacteristicPatterns.AddRangeAsync(patternsToCreateEntities);
			await _dbContext.SaveChangesAsync();

			return patternsToCreate;
		}

		public async Task<int> ChangeNameAsync(Guid patternId, string newName)
		{
			int updatedPatterns = await _dbContext.CharacteristicPatterns
				.Where(p => p.Id == patternId)
				.ExecuteUpdateAsync(p => p
				.SetProperty(p => p.Name, newName));

			return updatedPatterns;
		}

		public async Task<int> DeletePatternById(Guid patternId)
		{
			int deletedPatterns = await _dbContext.CharacteristicPatterns
				.Where(p => p.Id == patternId)
				.ExecuteDeleteAsync();

			await _dbContext.SaveChangesAsync();

			return deletedPatterns;
		}

		public async Task<int> DeleteCategoryPatternsByCategoryIdAsync(Guid categoryId)
		{
			CategoryEntity patternsOwner = await _dbContext.Categories
				.Include(p => p.CharacteristicPatterns)
				.FirstOrDefaultAsync(c => c.Id == categoryId);
				
			if(patternsOwner == null)
				return 0;
				
			int deletedPatterns = await _dbContext.CharacteristicPatterns
					.Where(p => patternsOwner.CharacteristicPatterns.Contains(p))
					.ExecuteDeleteAsync();
					
			return deletedPatterns;
		}
	}
}
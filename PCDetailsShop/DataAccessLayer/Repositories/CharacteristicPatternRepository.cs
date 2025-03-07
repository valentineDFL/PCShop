using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
	class CharacteristicPatternRepository : ICharacteristicPatternRepository
	{
		private readonly PcShopDbContext _dbContext;

		public CharacteristicPatternRepository(PcShopDbContext dbContext)
		{
			_dbContext = dbContext;
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
			Category patternsOwner = await _dbContext.Categories
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
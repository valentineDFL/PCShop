﻿using Domain.Models;

namespace Domain.Interfaces.Repositories
{
	public interface ICharacteristicPatternRepository
	{
		public Task<int> ChangeNameAsync(Guid patternId, string newName);

		public Task<int> DeletePatternById(Guid patternId);
		
		public Task<int> DeleteCategoryPatternsByCategoryIdAsync(Guid categoryId);
	}
}
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
	internal class CategoryRepository : ICategoryRepository
	{
		private readonly PcShopDbContext _dbContext;

		public CategoryRepository(PcShopDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Category>> GetAllAsync()
		{
			List<Category> categoties = await _dbContext.Categories
				.AsNoTracking()
				.Include(c => c.CharacteristicPatterns)
				.ToListAsync();

			if(categoties.Count == 0)
				return new List<Category>();

			return categoties;
		}

		public async Task<(Category category, ErrorCodes errorCode)> GetByIdAsync(Guid id)
		{
			Category categoryEntity = await _dbContext.Categories
                .AsNoTracking()
                .Include(c => c.CharacteristicPatterns)
				.FirstOrDefaultAsync(c => c.Id == id);

			if (categoryEntity == null)
				return (null, ErrorCodes.CategoryNotFound);

			return (categoryEntity, ErrorCodes.None);
		}

		public async Task<(Category category, ErrorCodes errorCode)> GetByNameAsync(string name)
		{
			Category categoryEntity = await _dbContext.Categories
				.Where(c => c.Name == name)
                .AsNoTracking()
                .Include(c => c.CharacteristicPatterns)
				.FirstOrDefaultAsync();

            if (categoryEntity == null)
				return (null, ErrorCodes.CategoryNotFound);

			return (categoryEntity, ErrorCodes.None);
		}

		public async Task<List<Category>> GetByNamesAsync(List<string> categoriesNames) 
		{
			List<Category> categoryEntities = await _dbContext.Categories
				.Where(c => categoriesNames.Any(n => n == c.Name))
				.AsNoTracking()
                .Include(c => c.CharacteristicPatterns)
				.ToListAsync();

			if (categoryEntities.Count == 0)
				return new List<Category>();

			return categoryEntities;
		}

		public async Task<Category> CreateAsync(Category category)
		{
			await _dbContext.AddAsync(category);
			await _dbContext.SaveChangesAsync();
			
			return category;
		}

		public async Task<int> DeleteByIdAsync(Guid id)
		{
			int deletedCategories = await _dbContext.Categories
				.Where(cat => cat.Id == id)
				.ExecuteDeleteAsync();

			await _dbContext.SaveChangesAsync();

			return deletedCategories;
		}

		public async Task<int> ChangeNameAsync(Guid id, string newName)
		{
			int updatedCategories = await _dbContext.Categories
				.Where(cat => cat.Id == id)
				.ExecuteUpdateAsync(c => c
				.SetProperty(c => c.Name, newName));

			await _dbContext.SaveChangesAsync();

			return updatedCategories;
		}
	}
}
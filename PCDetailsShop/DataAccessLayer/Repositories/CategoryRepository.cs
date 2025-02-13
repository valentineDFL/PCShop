using DataAccessLayer.Entities;
using DataAccessLayer.Mapping;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
	internal class CategoryRepository : ICategoryRepository
	{
		private readonly PcShopDbContext _dbContext;
		private readonly CategoryMapper _categoryMapper;

		public CategoryRepository(PcShopDbContext dbContext, CategoryMapper categoryMapper)
		{
			_dbContext = dbContext;
			_categoryMapper = categoryMapper;
		}

		public async Task<List<Category>> GetAllAsync()
		{
			List<CategoryEntity> categoties = await _dbContext.Categories
				.Include(c => c.CharacteristicPatterns)
				.ToListAsync();

			if(categoties.Count == 0)
				return new List<Category>();

			List<Category> mappedCategories = _categoryMapper.EntitiesToModels(categoties);
			
			System.Console.WriteLine(mappedCategories[0].CharacteristicPatterns.Count);
			
			return mappedCategories;
		}

		public async Task<(Category category, ErrorCodes errorCode)> GetByIdAsync(Guid id) // (Category, ErrorCode)
		{
			CategoryEntity categoryEntity = await _dbContext.Categories
				.Include(c => c.CharacteristicPatterns)
				.FirstOrDefaultAsync(c => c.Id == id);

			if (categoryEntity == null)
				return (null, ErrorCodes.CategoryNotFound);

			Category mappedCategory =  _categoryMapper.EntityToModel(categoryEntity);

			return (mappedCategory, ErrorCodes.None);
		}

		public async Task<List<Category>> GetByNameAsync(string name)
		{
			List<CategoryEntity> categoryEntities = await _dbContext.Categories
				.Include(c => c.CharacteristicPatterns)
				.Where(c => c.Name.Contains(name))
				.ToListAsync();

			if(categoryEntities.Count == 0)
				return new List<Category>();

			List<Category> mappedCategories = _categoryMapper.EntitiesToModels(categoryEntities);

			return mappedCategories;
		}

		public async Task<Category> CreateAsync(Category category)
		{
			CategoryEntity entity = _categoryMapper.ModelToEntity(category);

			await _dbContext.AddAsync(entity);
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
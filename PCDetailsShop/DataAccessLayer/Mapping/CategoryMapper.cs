using DataAccessLayer.Entities;
using Domain.Models;
using DataAccessLayer.Entities.Characteristic;

namespace DataAccessLayer.Mapping
{
	internal class CategoryMapper
	{
		private readonly CharacteristicPatternMapper _characteristicPatternMapper;

		public CategoryMapper(CharacteristicPatternMapper characteristicPatternMapper)
		{
			_characteristicPatternMapper = characteristicPatternMapper;
		}

		public Category EntityToModel(CategoryEntity categoryEntity)
		{
			if (categoryEntity == null) 
				throw new ArgumentNullException($"Category Entity is null {nameof(EntityToModel)}");

			return new Category
				(
					categoryEntity.Id,
					categoryEntity.Name,
					new List<Product>(),
					_characteristicPatternMapper.EntitiesToModels(categoryEntity.CharacteristicPatterns)
				);
		}

		public List<Category> EntitiesToModels(List<CategoryEntity> categoryEntities)
		{
			if (categoryEntities == null) 
				throw new ArgumentNullException($"Category Entities is null {nameof(EntitiesToModels)}");

			List<Category> result = new List<Category>();

			foreach (CategoryEntity categoryEntity in categoryEntities)
			{
				result.Add(EntityToModel(categoryEntity));
			}

			return result;
		}

		public CategoryEntity ModelToEntity(Category category)
		{
			if (category == null)
				throw new ArgumentNullException($"Category is null {nameof(ModelToEntity)}");

			CategoryEntity categoryEntity = new CategoryEntity()
			{
				Id = category.Id,
				Name = category.Name,
				Products = new List<ProductEntity>(),
				CharacteristicPatterns = _characteristicPatternMapper.ModelsToEntities(category.CharacteristicPatterns.ToList())
			};

			return categoryEntity;
		}

		public List<CategoryEntity> ModelsToEntities(List<Category> categories)
		{
			if (categories == null)
				throw new ArgumentNullException($"Categories is null {nameof(ModelsToEntities)}");

			List<CategoryEntity> result = new List<CategoryEntity>();

			foreach (Category category in categories)
			{
				result.Add(ModelToEntity(category));
			}

			return result;
		}
	}
}
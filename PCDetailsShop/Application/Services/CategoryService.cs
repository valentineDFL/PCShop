using System.Reflection.Metadata;
using System.Threading.Tasks;
using Domain.Dto.CategoryDtos;
using Domain.Dto.CharacteristicPatternDto;
using Domain.Dto.ProductDtos;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Interfaces.Validators;
using Domain.Models;
using Domain.Result;
using Serilog;

namespace Application.Services
{
	internal class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly ICharacteristicPatternRepository _characteristicPatternRepository;

		public CategoryService(ICategoryRepository categoryRepository, ICharacteristicPatternRepository characteristicPatternRepository)
		{
			_categoryRepository = categoryRepository;
			_characteristicPatternRepository = characteristicPatternRepository;
		}

		public async Task<CollectionResult<Category>> GetAllAsync()
		{
			List<Category> categories = await _categoryRepository.GetAllAsync();

			if(categories.Count == 0)
			{
				return new CollectionResult<Category>()
				{
					ErrorCode = (int)ErrorCodes.CategoriesNotFound,
					ErrorMessage = ErrorCodes.CategoryNotFound.ToString(),
				};
			}

			return new CollectionResult<Category>() { Data = categories };
		}

		public async Task<BaseResult<Category>> GetByIdAsync(Guid categoryId)
		{
			(Category category, ErrorCodes errorCode) findedCategory = await _categoryRepository.GetByIdAsync(categoryId);

			if(findedCategory.errorCode != ErrorCodes.None)
			{
				return new BaseResult<Category>()
				{
					ErrorCode = (int)findedCategory.errorCode,
					ErrorMessage = findedCategory.errorCode.ToString(),
				};
			}

			return new BaseResult<Category>() { Data = findedCategory.category };
		}

		public async Task<BaseResult<Category>> GetByNameAsync(string name)
		{
			(Category category, ErrorCodes errorCode) findedCategory = await _categoryRepository.GetByNameAsync(name);

			if (findedCategory.errorCode != ErrorCodes.None)
			{
				return new BaseResult<Category>()
				{
					ErrorCode = (int)ErrorCodes.CategoryNotFound,
					ErrorMessage = ErrorCodes.CategoryNotFound.ToString(),
				};
			}

			return new BaseResult<Category>() { Data = findedCategory.category };
		}

		public async Task<BaseResult<Category>> CreateAsync(CreateCategoryDto dto)
		{
			(Category category, ErrorCodes errorCode) findedCategory = await _categoryRepository.GetByNameAsync(dto.Name);

			if(findedCategory.errorCode == ErrorCodes.None)
			{
				return new BaseResult<Category>()
				{
					ErrorCode = (int)ErrorCodes.CategoryWithTurnedNameAlreadyExists,
					ErrorMessage = ErrorCodes.CategoryWithTurnedNameAlreadyExists.ToString()
				};
			}
			
			Guid categoryId = Guid.NewGuid();

			List<CharacteristicPattern> characteristicPatterns = CreatePatterns(dto.CharacteristicsToCreate, categoryId);

			Category categoryToCreate = new Category(categoryId, dto.Name, new List<Product>(), characteristicPatterns);
			
			Category createdCategory = await _categoryRepository.CreateAsync(categoryToCreate);
			
			return new BaseResult<Category>() { Data = createdCategory };
		}

		private List<CharacteristicPattern> CreatePatterns(List<CharacteristicPatternCreateDto> patternsToCreate, Guid categoryId)
		{
			List<CharacteristicPattern> characteristics = new List<CharacteristicPattern>();

			for (int index = 0; index < patternsToCreate.Count; index++)
				characteristics.Add(new CharacteristicPattern(Guid.NewGuid(), patternsToCreate[index].Name, categoryId, null, new List<CharacteristicRealization>()));

			return characteristics;
		}

		public async Task<BaseResult<Guid>> DeleteByIdAsync(Guid categoryId)
		{
			int deletedPatternsCount = await _characteristicPatternRepository.DeleteCategoryPatternsByCategoryIdAsync(categoryId);
			
			if(deletedPatternsCount == 0)
			{
				return new BaseResult<Guid>()
				{
					ErrorCode = (int)ErrorCodes.CategoryNotFound,
					ErrorMessage = ErrorCodes.CategoryNotFound.ToString(),
				};
			}
			
			int deletedCategoriesCount = await _categoryRepository.DeleteByIdAsync(categoryId);

			return new BaseResult<Guid>() { Data = categoryId };
		}

		public async Task<BaseResult<string>> ChangeNameByIdAsync(Guid categoryId, string newName)
		{
			(Category category, ErrorCodes errorCode) findedCategory = await _categoryRepository.GetByNameAsync(newName);

			if(findedCategory.errorCode == ErrorCodes.None)
			{
				return new BaseResult<string>()
				{
					ErrorCode = (int)ErrorCodes.CategoryWithTurnedNameAlreadyExists,
					ErrorMessage = ErrorCodes.CategoryWithTurnedNameAlreadyExists.ToString(),
				};
			}

			int categoriesWithChangedName = await _categoryRepository.ChangeNameAsync(categoryId, newName);

			if(categoriesWithChangedName == 0)
			{
				return new BaseResult<string>()
				{
					ErrorCode = (int)ErrorCodes.CategoryNotFound,
					ErrorMessage = ErrorCodes.CategoryNotFound.ToString(),
				};
			}

			return new BaseResult<string>() { Data = newName };
		}

		public async Task<CollectionResult<CharacteristicPattern>> GetCharacteristicsByCategoryIdAsync(Guid categoryId)
		{
			(Category category, ErrorCodes errorCode) findedCategory = await _categoryRepository.GetByIdAsync(categoryId);

			if(findedCategory.errorCode != ErrorCodes.None)
			{
				return new CollectionResult<CharacteristicPattern>()
				{
					ErrorCode = (int)findedCategory.errorCode,
					ErrorMessage = findedCategory.errorCode.ToString(),
				};
			}

			if(findedCategory.category.CharacteristicPatterns.Count == 0)
			{
				return new CollectionResult<CharacteristicPattern>()
				{
					ErrorCode = (int)ErrorCodes.CharacteristicsNotFound,
					ErrorMessage = ErrorCodes.CharacteristicsNotFound.ToString(),
				};
			}

			return new CollectionResult<CharacteristicPattern>() 
			{ 
				Count = findedCategory.category.CharacteristicPatterns.Count, 
				Data = (List<CharacteristicPattern>)findedCategory.category.CharacteristicPatterns
            };
		}

		public async Task<BaseResult<string>> ChangeCharacteristicNameByNameAsync(Guid categoryId, string characteristicPatternName, string newCharacteristicPatternName)
		{
			(Category category, ErrorCodes errorCode) findedCategory = await _categoryRepository.GetByIdAsync(categoryId);

			if(findedCategory.errorCode != ErrorCodes.None)
			{
				return new BaseResult<string>()
				{
					ErrorCode = (int)findedCategory.errorCode,
					ErrorMessage = findedCategory.errorCode.ToString(),
				};
			}

			Category category = findedCategory.category;
			
			CharacteristicPattern patternWithTurnedName = category.CharacteristicPatterns.FirstOrDefault(chara => chara.Name == newCharacteristicPatternName);
			if(patternWithTurnedName != null)
			{
				return new BaseResult<string>()
				{
					ErrorCode = (int)ErrorCodes.CharacteristicWithTurnedNameAlreadyExists,
					ErrorMessage = ErrorCodes.CharacteristicWithTurnedNameAlreadyExists.ToString()
				};
			}

			CharacteristicPattern patternToUpdate = category.CharacteristicPatterns.FirstOrDefault(chara => chara.Name == characteristicPatternName);
			if(patternToUpdate == null)
			{
				return new BaseResult<string>()
				{
					ErrorCode = (int)ErrorCodes.CharacteristicNotFound,
					ErrorMessage = ErrorCodes.CharacteristicNotFound.ToString(),
				};
			}

			int updatedCharacteristic = await _characteristicPatternRepository.ChangeNameAsync(patternToUpdate.Id, newCharacteristicPatternName);

			if(updatedCharacteristic == 0)
			{
				return new BaseResult<string>()
				{
					ErrorCode = (int)ErrorCodes.CategoryDoesNotContainSelectedCharacteristic,
					ErrorMessage = ErrorCodes.CategoryDoesNotContainSelectedCharacteristic.ToString(),
				};
			}

			return new BaseResult<string>() { Data = newCharacteristicPatternName };
		}
	}
}
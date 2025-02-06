using Domain.Dto.CategoryDtos;
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
        private readonly IProductRepository _productRepository;
        private readonly ICharacteristicPatternRepository _characteristicPatternRepository;

        private readonly ICategoryValidator _categoryValidator;

        private readonly ILogger _logger;

        public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository, ICharacteristicPatternRepository characteristicPatternRepository, ICategoryValidator categoryValidator, ILogger logger)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _characteristicPatternRepository = characteristicPatternRepository;
            _categoryValidator = categoryValidator;
            _logger = logger;
        }

        public async Task<CollectionResult<Category>> GetAllAsync()
        {
            List<Category> categories = await _categoryRepository.GetAllAsync();

            if(categories.Count == 0)
            {
                _logger.Warning(ErrorCodes.CategoriesNotFound.ToString());

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
            Category category = await _categoryRepository.GetByIdAsync(categoryId);

            if(category == null)
            {
                return new BaseResult<Category>()
                {
                    ErrorCode = (int)ErrorCodes.CategoryNotFound,
                    ErrorMessage = ErrorCodes.CategoryNotFound.ToString(),
                };
            }

            return new BaseResult<Category>() { Data = category };
        }

        public async Task<CollectionResult<Category>> GetByNamePartAsync(string namePart)
        {
            List<Category> categories = await _categoryRepository.GetByNamePartAsync(namePart);

            if (categories.Count == 0)
            {
                return new CollectionResult<Category>()
                {
                    ErrorCode = (int)ErrorCodes.CategoriesNotFound,
                    ErrorMessage = ErrorCodes.CategoriesNotFound.ToString(),
                };
            }

            return new CollectionResult<Category>() { Data = categories };
        }

        public async Task<BaseResult<Category>> GetByNameAsync(string name)
        {
            Category category = await _categoryRepository.GetByNameAsync(name);

            if (category == null)
            {
                return new BaseResult<Category>()
                {
                    ErrorCode = (int)ErrorCodes.CategoryNotFound,
                    ErrorMessage = ErrorCodes.CategoryNotFound.ToString(),
                };
            }

            return new BaseResult<Category>() { Data = category };
        }

        public async Task<CollectionResult<Product>> GetAllCategoryProductsById(Guid categoryId)
        {
            Category category = await _categoryRepository.GetByIdAsync(categoryId);

            if(category == null)
            {
                return new CollectionResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.CategoryNotFound,
                    ErrorMessage = ErrorCodes.CategoryNotFound.ToString(),
                };
            }

            if(category.Products.Count == 0)
            {
                return new CollectionResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.ProductsNotFound,
                    ErrorMessage = ErrorCodes.ProductsNotFound.ToString(),
                };
            }

            return new CollectionResult<Product>() {Count = category.Products.Count, Data = category.Products };
        }

        public async Task<CollectionResult<Product>> GetAllCategoryProductsByName(string name)
        {
            Category category = await _categoryRepository.GetByNameAsync(name);

            if (category == null)
            {
                return new CollectionResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.CategoryNotFound,
                    ErrorMessage = ErrorCodes.CategoryNotFound.ToString(),
                };
            }

            if (category.Products.Count == 0)
            {
                _logger.Warning(ErrorCodes.ProductsNotFound.ToString());

                return new CollectionResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.ProductsNotFound,
                    ErrorMessage = ErrorCodes.ProductsNotFound.ToString(),
                };
            }

            return new CollectionResult<Product>() { Count = category.Products.Count, Data = category.Products };
        }

        public Task<BaseResult<Category>> CreateAsync(CreateCategoryDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResult<Guid>> DeleteByIdAsync(Guid categoryId)
        {
            int deletedCategoriesCount = await _categoryRepository.DeleteByIdAsync(categoryId);

            if(deletedCategoriesCount == 0)
            {
                return new BaseResult<Guid>()
                {
                    ErrorCode = (int)ErrorCodes.CategoryNotFound,
                    ErrorMessage = ErrorCodes.CategoryNotFound.ToString(),
                };
            }

            return new BaseResult<Guid>() { Data = categoryId };
        }

        public async Task<BaseResult<Category>> ChangeNameAsync(Guid categoryId, string newName)
        {
            Category category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null)
            {
                return new BaseResult<Category>()
                {
                    ErrorCode = (int)ErrorCodes.CategoryNotFound,
                    ErrorMessage = ErrorCodes.CategoryNotFound.ToString(),
                };
            }

            Category categoryWithTurnedNewName = await _categoryRepository.GetByNameAsync(newName);

            BaseResult<Category> categoryNewNameExistsValidationResult = _categoryValidator.ValidateOnNameExists(categoryWithTurnedNewName);

            if (!categoryNewNameExistsValidationResult.IsSuccess)
                return categoryNewNameExistsValidationResult;

            await _categoryRepository.ChangeNameAsync(categoryId, newName);

            return new BaseResult<Category>() { Data = category };
        }



        public async Task<CollectionResult<Product>> AddProductsToCategoryAsync(Guid categoryId, List<Guid> productsId)
        {
            Category category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null)
            {
                return new CollectionResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.CategoryNotFound,
                    ErrorMessage = ErrorCodes.CategoryNotFound.ToString(),
                };
            }

            int updatedCategories = await _categoryRepository.AddProductsToCategoryAsync(categoryId, productsId);

            

            throw new NotImplementedException();
        }

        public Task<CollectionResult<Product>> DeleteProductsFromCategoryAsync(Guid categoryId, List<Guid> productsId)
        {
            throw new NotImplementedException();
        }

        public async Task<CollectionResult<CharacteristicPattern>> GetCharacteristicsByCategoryIdAsync(Guid categoryId)
        {
            Category category = await _categoryRepository.GetByIdAsync(categoryId);

            if(category == null)
            {
                return new CollectionResult<CharacteristicPattern>()
                {
                    ErrorCode = (int)ErrorCodes.CategoryNotFound,
                    ErrorMessage = ErrorCodes.CategoryNotFound.ToString(),
                };
            }

            return new CollectionResult<CharacteristicPattern>() { Count = category.CharacteristicPatterns.Count, Data = category.CharacteristicPatterns };
        }

        public async Task<BaseResult<CharacteristicPattern>> ChangeCharacteristicNameByNameAsync(Guid categoryId, string characteristicPatternName, string newCharacteristicPatternName)
        {
            Category category = await _categoryRepository.GetByIdAsync(categoryId);

            if(category == null)
            {
                return new BaseResult<CharacteristicPattern>()
                {
                    ErrorCode = (int)ErrorCodes.CharacteristicNotFound,
                    ErrorMessage = ErrorCodes.CharacteristicNotFound.ToString(),
                };
            }


        }

        public Task<CharacteristicPattern> AddCharacteristicsToCategoryAsync(Guid categoryId, List<Guid> characteristicsId)
        {
            throw new NotImplementedException();
        }

        public Task<CharacteristicPattern> DeleteCharacteristicsFromCategoryAsync(Guid categoryId, List<Guid> characteristicsId)
        {
            throw new NotImplementedException();
        }
    }
}
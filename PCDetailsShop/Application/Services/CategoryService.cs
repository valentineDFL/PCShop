using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.CategoryDtos;
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
        private readonly IRepository<Product> _productRepository;

        private readonly ICategoryValidator _categoryValidator;

        private readonly ILogger _logger;

        public CategoryService(ICategoryRepository categoryRepository, IRepository<Product> productRepository, ICategoryValidator categoryValidator, ILogger logger)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _categoryValidator = categoryValidator;
            _logger = logger;
        }

        public async Task<CollectionResult<Category>> GetAllAsync()
        {
            try
            {
                CollectionResult<Category> categories = await _categoryRepository.GetAllAsync();
                
                return categories;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new CollectionResult<Category>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString(),
                };
            }
        }

        public async Task<BaseResult<Category>> GetByIdAsync(Guid categoryId)
        {
            try
            {
                BaseResult<Category> category = await _categoryRepository.GetByIdAsync(categoryId);

                return category;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<Category>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString(),
                };
            }
        }

        public async Task<CollectionResult<Product>> GetAllCategoryProducts(string categoryNamePart)
        {
            try
            {
                BaseResult<Category> category = await _categoryRepository.GetByNameAsync(categoryNamePart);

                if (!category.IsSuccess)
                {
                    return new CollectionResult<Product>()
                    {
                        ErrorCode = category.ErrorCode,
                        ErrorMessage = category.ErrorMessage,
                    };
                }

                return new CollectionResult<Product>()
                {
                    Count = category.Data.Products.Count,
                    Data = category.Data.Products
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new CollectionResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage= ErrorCodes.InternalServerError.ToString(),
                };
            }
        }

        public async Task<BaseResult<Category>> GetByNamePartAsync(string namePart)
        {
            try
            {
                BaseResult<Category> category = await _categoryRepository.GetByNameAsync(namePart);

                return category;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<Category>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString(),
                };
            }
        }

        public async Task<BaseResult<Category>> CreateAsync(CreateCategoryDto dto)
        {
            try
            {
                BaseResult<Category> category = await _categoryRepository.GetByNameAsync(dto.Name);

                BaseResult<Category> categoryExistsValidationResult = _categoryValidator.ValidateOnNameExists(category.Data);

                if (!categoryExistsValidationResult.IsSuccess)
                    return categoryExistsValidationResult;

                Guid categoryId = Guid.NewGuid();

                Category categoryToCreate = new Category(categoryId, dto.Name, dto.Products, dto.Characteristics);

                BaseResult<Category> createdCategory = await _categoryRepository.CreateAsync(categoryToCreate);

                return createdCategory;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<Category>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString(),
                };
            }
        }

        public async Task<BaseResult<Guid>> DeleteByIdAsync(Guid categoryId)
        {
            try
            {
                BaseResult<Guid> category = await _categoryRepository.DeleteAsync(categoryId);

                return category;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<Guid>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString(),
                };
            }
        }

        public async Task<BaseResult<Category>> ChangeNameAsync(Guid categoryId, string newName)
        {
            try
            {
                BaseResult<Category> category = await _categoryRepository.GetByIdAsync(categoryId);

                if (!category.IsSuccess)
                    return category;

                BaseResult<Category> categoryWithTurnedNewName = await _categoryRepository.GetByNameAsync(newName);

                BaseResult<Category> categoryNameExistsValidationResult = _categoryValidator.ValidateOnNameExists(categoryWithTurnedNewName.Data);

                if (!categoryNameExistsValidationResult.IsSuccess)
                    return categoryNameExistsValidationResult;

                Category updatedCategory = new Category(category.Data.Id, newName, category.Data.Products, category.Data.Characteristics);

                return await _categoryRepository.UpdateAsync(updatedCategory);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<Category>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString(),
                };
            }
        }

        public async Task<BaseResult<Product>> AddProductToCategoryAsync(string categoryName, Guid productId)
        {
            try
            {
                BaseResult<Category> category = await _categoryRepository.GetByNameAsync(categoryName);

                if (!category.IsSuccess)
                {
                    return new BaseResult<Product>()
                    {
                        ErrorCode = category.ErrorCode,
                        ErrorMessage = category.ErrorMessage,
                    };
                }

                BaseResult<Product> productToAdd = await _productRepository.GetByIdAsync(productId);

                if (!productToAdd.IsSuccess)
                    return productToAdd;

                BaseResult<Product> validateOnProductRepeatResult = _categoryValidator.ValidateOnProductRepeat(category.Data, productToAdd.Data);

                if (!validateOnProductRepeatResult.IsSuccess)
                    return validateOnProductRepeatResult;

                return await AddProductToCategoryAsync(category, productToAdd);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString(),
                };
            }
        }

        private async Task<BaseResult<Product>> AddProductToCategoryAsync(BaseResult<Category> category, BaseResult<Product> productToAdd)
        {
            List<Product> products = category.Data.Products.ToList();
            products.Add(productToAdd.Data);

            Category updatedCategory = new Category(category.Data.Id, category.Data.Name, products, category.Data.Characteristics);

            BaseResult<Category> updateResult = await _categoryRepository.UpdateAsync(updatedCategory);

            return productToAdd;
        }

        public async Task<BaseResult<Product>> DeleteProductFromCategoryAsync(string categoryNamePart, Guid productId)
        {
            try
            {
                BaseResult<Category> category = await _categoryRepository.GetByNameAsync(categoryNamePart);

                if (!category.IsSuccess)
                {
                    return new BaseResult<Product>()
                    {
                        ErrorCode = category.ErrorCode,
                        ErrorMessage = category.ErrorMessage,
                    };
                }

                BaseResult<Product> productToRemoveResult = await _productRepository.GetByIdAsync(productId);

                if(!productToRemoveResult.IsSuccess)
                    return productToRemoveResult;

                BaseResult<Product> validateOnProductExitstInCategoryResult = _categoryValidator.ValidateOnProductExistsInCategory(category.Data, productToRemoveResult.Data);

                if(!validateOnProductExitstInCategoryResult.IsSuccess)
                    return validateOnProductExitstInCategoryResult;

                return await RemoveProductFromCategoryAsync(category, productToRemoveResult.Data);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

                return new BaseResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorCodes.InternalServerError.ToString(),
                };
            }
        }
        
        private async Task<BaseResult<Product>> RemoveProductFromCategoryAsync(BaseResult<Category> category, Product productToRemove)
        {
            List<Product> products = category.Data.Products.ToList();
            products.Remove(productToRemove);

            Category updatedCategory = new Category(category.Data.Id, category.Data.Name, products, category.Data.Characteristics);

            BaseResult<Category> updateResult = await _categoryRepository.UpdateAsync(updatedCategory);

            return new BaseResult<Product>() { Data = productToRemove };
        }
    }
}
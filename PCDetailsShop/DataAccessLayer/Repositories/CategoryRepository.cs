using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using DataAccessLayer.Mapping;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Result;
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

        public async Task<CollectionResult<Category>> GetAllAsync()
        {
            List<CategoryEntity> categoties = await _dbContext.Categories
                .Include(c => c.Products)
                .ToListAsync();

            if(categoties.Count == 0)
            {
                return new CollectionResult<Category>()
                {
                    ErrorCode = (int)ErrorCodes.CategoriesNotFound,
                    ErrorMessage = ErrorCodes.CategoriesNotFound.ToString()
                };
            }

            List<Category> result = await _categoryMapper.EntitiesToModelsAsync(categoties);
            
            return new CollectionResult<Category>()
            {
                Count = result.Count,
                Data = result
            };
        }

        public async Task<BaseResult<Category>> GetByIdAsync(Guid id)
        {
            CategoryEntity categoryEntity = await _dbContext.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if(categoryEntity == null)
            {
                return new BaseResult<Category>()
                {
                    ErrorCode = (int)ErrorCodes.CategoryNotFound,
                    ErrorMessage= ErrorCodes.CategoryNotFound.ToString()
                };
            }

            Category category = _categoryMapper.EntityToModel(categoryEntity);

            return new BaseResult<Category>() { Data = category };
        }

        public async Task<BaseResult<Category>> GetByNameAsync(string partName)
        {
            CategoryEntity categoryEntity = await _dbContext.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Name.Contains(partName));

            if (categoryEntity == null)
            {
                return new BaseResult<Category>()
                {
                    ErrorCode = (int)ErrorCodes.CategoryNotFound,
                    ErrorMessage = ErrorCodes.CategoryNotFound.ToString()
                };
            }

            Category category = _categoryMapper.EntityToModel(categoryEntity);

            return new BaseResult<Category>() { Data = category };
        }

        public async Task<BaseResult<Category>> CreateAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException($"Categoty null {nameof(CreateAsync)}");

            CategoryEntity entity = _categoryMapper.ModelToEntity(category);

            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return new BaseResult<Category>() { Data = category };
        }

        public async Task<BaseResult<Guid>> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException($"Category id is Empty {nameof(DeleteAsync)}");

            int deletedCategories = await _dbContext.Categories
                .Where(cat => cat.Id == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();

            if (deletedCategories == 0)
            {
                return new BaseResult<Guid>()
                {
                    ErrorCode = (int)ErrorCodes.CategoryNotFound,
                    ErrorMessage = ErrorCodes.CategoryNotFound.ToString()
                };
            }

            return new BaseResult<Guid>() { Data = id };
        }

        public async Task<BaseResult<Category>> UpdateAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException($"Categoty null {nameof(UpdateAsync)}");

            CategoryEntity entity = _categoryMapper.ModelToEntity(category);

            int updatedCategoriesCount = await _dbContext.Categories
                .Where(c => c.Id == entity.Id)
                .ExecuteUpdateAsync(c => c
                .SetProperty(p => p.Name, entity.Name)
                .SetProperty(p => p.Products, entity.Products));

            if(updatedCategoriesCount == 0)
            {
                return new BaseResult<Category>()
                {
                    ErrorCode = (int)ErrorCodes.CategoryNotFound,
                    ErrorMessage = ErrorCodes.CategoryNotFound.ToString()
                };
            }

            await _dbContext.AddAsync(entity);

            return new BaseResult<Category>() { Data = category };
        }
    }
}
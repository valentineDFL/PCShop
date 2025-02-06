using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        public async Task<List<Category>> GetAllAsync()
        {
            List<CategoryEntity> categoties = await _dbContext.Categories
                .Include(c => c.Products)
                .Include(c => c.CharacteristicPatterns)
                .ToListAsync();

            List<Category> mappedCategories = await _categoryMapper.EntitiesToModelsAsync(categoties);
            
            return mappedCategories;
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            CategoryEntity categoryEntity = await _dbContext.Categories
                .Include(c => c.Products)
                .Include(c => c.CharacteristicPatterns)
                .FirstOrDefaultAsync(c => c.Id == id);

            Category mappedCategory = _categoryMapper.EntityToModel(categoryEntity);

            return mappedCategory;
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            CategoryEntity categoryEntity = await _dbContext.Categories
                .Include(c => c.Products)
                .Include(c => c.CharacteristicPatterns)
                .FirstOrDefaultAsync(c => c.Name == name);

            Category mappedCategory = _categoryMapper.EntityToModel(categoryEntity);

            return mappedCategory;
        }

        public async Task<List<Category>> GetByNamePartAsync(string namePart)
        {
            List<CategoryEntity> categoryEntities = await _dbContext.Categories
                .Include(c => c.Products)
                .Include(c => c.CharacteristicPatterns)
                .Where(c => c.Name.Contains(namePart)
                ).ToListAsync();

            List<Category> mappedCategory = await _categoryMapper.EntitiesToModelsAsync(categoryEntities);

            return mappedCategory;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException($"Categoty null {nameof(CreateAsync)}");

            CategoryEntity entity = _categoryMapper.ModelToEntity(category);

            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            Category mappedCategory = _categoryMapper.EntityToModel(entity);

            return mappedCategory;
        }

        public async Task<int> DeleteByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException($"Category id is Empty {nameof(DeleteByIdAsync)}");

            int deletedCategories = await _dbContext.Categories
                .Where(cat => cat.Id == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();

            return deletedCategories;
        }

        public async Task<int> ChangeNameAsync(Guid id, string newName)
        {
            if (string.IsNullOrEmpty(newName))
                throw new ArgumentNullException($"NewCategoryName null or empty {nameof(ChangeNameAsync)}");

            int updatedCategories = await _dbContext.Categories
                .Where(cat => cat.Id == id)
                .ExecuteUpdateAsync(c => c
                .SetProperty(c => c.Name, newName));

            await _dbContext.SaveChangesAsync();

            return updatedCategories;
        }

        public async Task<int> AddProductsToCategoryAsync(Guid categoryId, List<Guid> productsId)
        {
            CategoryEntity category = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if(category == null)
                return 0;

            List<ProductEntity> productsToAdd = await _dbContext.Products
                .Where(prod => productsId.Contains(prod.Id))
                .ToListAsync();

            if(productsToAdd.Count == 0)
                return 0;

            List<ProductEntity> categoryProducts = GetAddedProducts(category, productsToAdd);

            int updatedCategories = await _dbContext.Categories
                .Where(cat => cat.Id == categoryId)
                .ExecuteUpdateAsync(c => c
                .SetProperty(c => c.Products, categoryProducts));

            await _dbContext.SaveChangesAsync();

            return updatedCategories;
        }

        private List<ProductEntity> GetAddedProducts(CategoryEntity category, List<ProductEntity> productsToAdd)
        {
            List<ProductEntity> categoryProducts = category.Products;

            foreach (ProductEntity productToAdd in productsToAdd)
            {
                if (categoryProducts.Contains(productToAdd))
                    productsToAdd.Remove(productToAdd);
            }

            categoryProducts.AddRange(productsToAdd);

            return categoryProducts;
        }

        public async Task<int> RemoveProductsFromCategoryAsync(Guid categoryId, List<Guid> productsId)
        {
            CategoryEntity category = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
                return 0;

            List<ProductEntity> productsToRemove = await _dbContext.Products
                .Where(prod => productsId.Contains(prod.Id))
                .ToListAsync();

            if (productsToRemove.Count == 0)
                return 0;

            List<ProductEntity> categoryProducts = category.Products;

            foreach (ProductEntity productToRemove in productsToRemove)
            {
                if (categoryProducts.Contains(productToRemove))
                    categoryProducts.Remove(productToRemove);
            }

            int updatedCategories = await _dbContext.Categories
                .Where(cat => cat.Id == categoryId)
                .ExecuteUpdateAsync(c => c
                .SetProperty(c => c.Products, categoryProducts));

            await _dbContext.SaveChangesAsync();

            return updatedCategories;
        }
    }
}
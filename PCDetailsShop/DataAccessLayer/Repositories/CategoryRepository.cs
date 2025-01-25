using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using DataAccessLayer.Mapping;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    internal class CategoryRepository // : IRepository<Category>
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
            List<CategoryEntity> categoties = await _dbContext.Categories.ToListAsync();
            List<Category> result = await _categoryMapper.EntitiesToModelsAsync(categoties);
            
            return result;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException($"Categoty null {nameof(CreateAsync)}");

            CategoryEntity entity = _categoryMapper.ModelToEntity(category);

            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            int deletedCategories = await _dbContext.Categories
                .Where(cat => cat.Id == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();

            if (deletedCategories == 0)
                throw new ArgumentException("Categoty not found");

            return id;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException($"Categoty null {nameof(UpdateAsync)}");

            CategoryEntity entity = _categoryMapper.ModelToEntity(category);

            await _dbContext.Categories
                .Where(c => c.Id == entity.Id)
                .ExecuteUpdateAsync(c => c
                .SetProperty(p => p.Name, entity.Name)
                .SetProperty(p => p.Products, entity.Products));

            await _dbContext.AddAsync(entity);

            return category;
        }
    }
}
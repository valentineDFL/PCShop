using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using DataAccessLayer.Mapping;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace DataAccessLayer.Repositories
{
    internal class ProductRepository // : IRepository<Product>
    {
        private readonly PcShopDbContext _dbContext;
        private readonly ProductMapper _productMapper;

        public ProductRepository(PcShopDbContext dbContext, ProductMapper productMapper)
        {
            _dbContext = dbContext;
            _productMapper = productMapper;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            List<ProductEntity> entities = await _dbContext.Products.ToListAsync();
            List<Product> products = await _productMapper.EntitiesToModelsAsync(entities);

            return products;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException($"Product null {nameof(CreateAsync)}");

            ProductEntity entity = await _productMapper.ModelToEntityAsync(product);

            await _dbContext.Products.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return product;
        }

        public async Task<Product> DeleteAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException($"Product null {nameof(DeleteAsync)}");

            ProductEntity entity = await _productMapper.ModelToEntityAsync(product);

            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException($"Product null {nameof(UpdateAsync)}");

            ProductEntity entity = await _productMapper.ModelToEntityAsync(product);

            await _dbContext.Products
                .Where(p => p.Id == entity.Id)
                .ExecuteUpdateAsync(p => p
                .SetProperty(p => p.Name, entity.Name)
                .SetProperty(p => p.Description, entity.Description)
                .SetProperty(p => p.Price, entity.Price)
                .SetProperty(p => p.Weight, entity.Weight)
                .SetProperty(p => p.Categories, entity.Categories));

            await _dbContext.SaveChangesAsync();

            return product;
        }
    }
}
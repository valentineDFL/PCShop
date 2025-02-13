using DataAccessLayer.Entities;
using DataAccessLayer.Mapping;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Result;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    internal class ProductRepository // : IProductRepository
    {
        private readonly PcShopDbContext _dbContext;
        private readonly ProductMapper _productMapper;

        public ProductRepository(PcShopDbContext dbContext, ProductMapper productMapper)
        {
            _dbContext = dbContext;
            _productMapper = productMapper;
        }

        public async Task<CollectionResult<Product>> GetAllAsync()
        {
            List<ProductEntity> entities = await _dbContext.Products.ToListAsync();

            if(entities.Count == 0)
            {
                return new CollectionResult<Product>()
                {
                    Count = 0,
                    ErrorCode = (int)ErrorCodes.ProductsNotFound,
                    ErrorMessage = ErrorCodes.ProductsNotFound.ToString()
                };
            }

            List<Product> products = await _productMapper.EntitiesToModelsAsync(entities);

            return new CollectionResult<Product>()
            {
                Count = products.Count,
                Data = products
            };
        }

        public async Task<BaseResult<Product>> CreateAsync(Product product)
        {
            ProductEntity entity = _productMapper.ModelToEntity(product);

            await _dbContext.Products.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return new BaseResult<Product>() { Data = product };
        }

        public async Task<BaseResult<Guid>> DeleteAsync(Guid id)
        {
            int countDeletedProducts = await _dbContext.Products
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();

            if(countDeletedProducts == 0)
            {
                return new BaseResult<Guid>()
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorCodes.ProductNotFound.ToString()
                };
            }

            await _dbContext.SaveChangesAsync();

            return new BaseResult<Guid>() { Data = id };
        }

        public async Task<BaseResult<Product>> UpdateAsync(Product product)
        {
            ProductEntity entity = _productMapper.ModelToEntity(product);

            int updatedProductsCount = await _dbContext.Products
                .Where(p => p.Id == entity.Id)
                .ExecuteUpdateAsync(p => p
                .SetProperty(p => p.Name, entity.Name)
                .SetProperty(p => p.Description, entity.Description)
                .SetProperty(p => p.Price, entity.Price)
                .SetProperty(p => p.Weight, entity.Weight)
                .SetProperty(p => p.Categories, entity.Categories));

            if(updatedProductsCount == 0)
            {
                return new BaseResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                    ErrorMessage = ErrorCodes.ProductNotFound.ToString()
                };
            }

            await _dbContext.SaveChangesAsync();

            return new BaseResult<Product>()
            {
                Data = product
            };
        }
    }
}
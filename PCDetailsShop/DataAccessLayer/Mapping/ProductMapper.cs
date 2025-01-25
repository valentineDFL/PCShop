using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using Domain.Models;

namespace DataAccessLayer.Mapping
{
    internal class ProductMapper
    {
        private readonly CategoryMapper _categoryMapper;

        public ProductMapper(CategoryMapper categoryMapper)
        {
            _categoryMapper = categoryMapper;
        }

        public async Task<ProductEntity> ModelToEntityAsync(Product product)
        {
            ProductEntity productEntity = new ProductEntity()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Weight = product.Weight,

                // сделать null продуктов у категорий, - когда буду маппить категории,
                // в методе MapCategoriesToEntities, если продукты == нулл,
                // то продуктам присваивать пустой список.
                Categories = await _categoryMapper.ModelsToEntitiesAsync((List<Category>)product.Categories),

            };

            return productEntity;
        }

        public Task<Product> EntityToModelAsyns(ProductEntity productEntity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductEntity>> ModelsToEntitiesAsync(List<Product> products)
        {
            List<ProductEntity> entities = new List<ProductEntity>();

            foreach(Product product in products)
            {
                entities.Add(await ModelToEntityAsync(product));
            }

            return entities;
        }

        public async Task<List<Product>> EntitiesToModelsAsync(List<ProductEntity> entities)
        {
            List<Product> products = new List<Product>();

            foreach(ProductEntity entity in entities)
            {
                products.Add(await EntityToModelAsyns(entity));
            }

            return products;
        }
    }
}
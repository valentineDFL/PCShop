using DataAccessLayer.Entities;
using Domain.Models;

namespace DataAccessLayer.Mapping
{
    internal class ProductMapper
    {
        private readonly CategoryMapper _categoryMapper;
        private readonly CharacteristicRealizationMapper _characteristicRealizationMapper;

        public ProductMapper(CategoryMapper categoryMapper, CharacteristicRealizationMapper characteristicRealizationMapper)
        {
            _categoryMapper = categoryMapper;
            _characteristicRealizationMapper = characteristicRealizationMapper;
        }

        public ProductEntity ModelToEntity(Product product)
        {
            if (product == null)
                throw new ArgumentNullException($"Product is null {nameof(ModelToEntity)}");

            return new ProductEntity()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Weight = product.Weight,
                Categories = _categoryMapper.ModelsToEntities(product.Categories.ToList()),
                StockAvailability = product.StockAvailability,
                Count = product.Count,
                CharacteristicsRealizations = _characteristicRealizationMapper.ModelsToEntities(product.CharacteristicsRelizations.ToList())
            };
        }

        public Product EntityToModel(ProductEntity productEntity)
        {
            if (productEntity == null)
                throw new ArgumentNullException($"Product entity is null {nameof(ModelToEntity)}");

            return new Product
                (
                    productEntity.Id,
                    productEntity.Name,
                    productEntity.Description,
                    productEntity.Price,
                    productEntity.Weight,
                    _categoryMapper.EntitiesToModels(productEntity.Categories),
                    _characteristicRealizationMapper.EntitiesToModels(productEntity.CharacteristicsRealizations),
                    productEntity.StockAvailability,
                    productEntity.Count
                );
        }

        public async Task<List<ProductEntity>> ModelsToEntitiesAsync(List<Product> products)
        {
            if (products == null)
                throw new ArgumentNullException($"Products is null {nameof(ModelsToEntitiesAsync)}");

            List<ProductEntity> entities = new List<ProductEntity>();

            await Task.Run(() =>
            {
                foreach(Product product in products)
                {
                    entities.Add(ModelToEntity(product));
                }
            });

            return entities;
        }

        public async Task<List<Product>> EntitiesToModelsAsync(List<ProductEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException($"Product entities is null {nameof(EntitiesToModelsAsync)}");

            List<Product> products = new List<Product>();

            await Task.Run(() =>
            {
                foreach (ProductEntity entity in entities)
                {
                    products.Add(EntityToModel(entity));
                }
            });

            return products;
        }
    }
}
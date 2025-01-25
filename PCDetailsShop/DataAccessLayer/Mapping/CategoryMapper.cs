using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using Domain.Models;

namespace DataAccessLayer.Mapping
{
    internal class CategoryMapper
    {
        public CategoryEntity ModelToEntity(Category category)
        {
            CategoryEntity categoryEntity = new CategoryEntity()
            {
                Id = category.Id,
                Name = category.Name,
                Products = new List<ProductEntity>(),
            };

            return categoryEntity;
        }

        public async Task<List<CategoryEntity>> ModelsToEntitiesAsync(List<Category> categories)
        { 
            List<CategoryEntity> result = new List<CategoryEntity>();

            await Task.Run(() => 
            {
                foreach (var category in categories)
                {
                    result.Add(ModelToEntity(category));
                }
            });

            return result;
        }

        public Category EntityToModel(CategoryEntity categoryEntity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Category>> EntitiesToModelsAsync(List<CategoryEntity> categoryEntities)
        {
            throw new NotImplementedException();
        }
    }
}
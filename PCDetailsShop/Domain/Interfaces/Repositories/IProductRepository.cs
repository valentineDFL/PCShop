using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllAsync();

        public Task<Product> GetByIdAsync(Guid id);

        public Task<Product> GetByNameAsync(string namePart);

        public Task<Product> CreateAsync(Product product);

        public Task<string> ChangeNameAsync(string newName);

        public Task<string> ChangeDescriptionAsync(string newDescription);

        public Task<decimal> ChangePriceAsync(decimal newPrice);

        public Task<float> ChangeWeightAsync(float newWeight);

        public Task<Category> AddCategoryToProductAsync(Guid priductId, Guid categoryId);

        public Task<Category> DeleteCategoryFromProductAsync(Guid priductId, Guid categoryId);


        public Task<int> DeleteByIdAsync(Guid id);


    }
}

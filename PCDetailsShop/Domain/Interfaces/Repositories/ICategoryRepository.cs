using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetAllAsync();
        
        public Task<Category> GetByIdAsync(Guid id);

        public Task<Category> GetByNameAsync(string partName);

        public Task<List<Category>> GetByNamePartAsync(string namePart);

        public Task<Category> CreateAsync(Category category);

        public Task<int> ChangeNameAsync(Guid id, string newName);

        public Task<int> AddProductsToCategoryAsync(Guid categoryId, List<Guid> productsId);

        public Task<int> RemoveProductsFromCategoryAsync(Guid categoryId, List<Guid> productsId);

        public Task<int> DeleteByIdAsync(Guid id);
    }
}
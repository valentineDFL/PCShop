using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetAllAsync();
        
        public Task<(Category category, ErrorCodes errorCode)> GetByIdAsync(Guid id);

        public Task<(Category category, ErrorCodes errorCode)> GetByNameAsync(string name);

        public Task<List<Category>> GetByNamesAsync(List<string> categoriesNames);

        public Task<Category> CreateAsync(Category category);

        public Task<int> ChangeNameAsync(Guid id, string newName);

        public Task<int> DeleteByIdAsync(Guid id);
    }
}
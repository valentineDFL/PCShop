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
        public Task<CollectionResult<Category>> GetAllAsync();
        
        public Task<BaseResult<Category>> GetByIdAsync(Guid id);

        public Task<BaseResult<Category>> GetByNameAsync(string partName);

        public Task<BaseResult<Category>> CreateAsync(Category category);

        public Task<BaseResult<Category>> UpdateAsync(Category category);

        public Task<BaseResult<Guid>> DeleteAsync(Guid id);
    }
}
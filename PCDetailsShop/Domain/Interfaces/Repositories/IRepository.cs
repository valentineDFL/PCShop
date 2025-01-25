using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Repositories
{
    public interface IRepository<TModel>
    {
        public Task<CollectionResult<TModel>> GetAllAsync();

        public Task<BaseResult<TModel>> CreateAsync(TModel model);

        public Task<BaseResult<TModel>> UpdateAsync(TModel model);

        public Task<BaseResult<Guid>> DeleteAsync(Guid id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.UserDtos;
using Domain.Result;

namespace Domain.Interfaces.Mapping
{
    public interface IDtoMapper<TModel, TDto>
    {
        public BaseResult<TDto> FromModelToDtoResult(TModel model);

        public Task<CollectionResult<TDto>> FromModelsToDtosAsync(List<TModel> models);
    }
}

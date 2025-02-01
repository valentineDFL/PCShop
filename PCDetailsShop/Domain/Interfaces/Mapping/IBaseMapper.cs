using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.UserDtos;
using Domain.Result;

namespace Domain.Interfaces.MappingW
{
    public interface IBaseMapper<TModel, TDto>
    {
        public BaseResult<TDto> FromModelToDto(TModel model);

        public Task<CollectionResult<UserDto>> FromModelsToDtosAsync(List<TModel> models);
    }
}

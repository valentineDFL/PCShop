using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.MappingW
{
    public interface IBaseMapper<TModel, TDto>
    {
        public TDto FromModelToDto(TModel model);

        public TModel FromDtoToModel(TDto dto);

        public Task<List<TDto>> FromModelsToDtosAsync(List<TModel> models);

        public Task<List<TModel>> FromDtosToModelsAsync(List<TDto> dtos);
    }
}

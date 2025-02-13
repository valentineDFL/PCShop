using System;
using Domain.Dto.CharacteristicPatternDto;
using Domain.Interfaces.Mapping;
using Domain.Models;
using Domain.Result;

namespace PCDetailsShop.API.DtoMapping
{
    public class CharacteristicPatternDtoMapper : IDtoMapper<CharacteristicPattern, CharacteristicPatternDto>
    {
        public BaseResult<CharacteristicPatternDto> FromModelToDtoResult(CharacteristicPattern model)
        {
            return new BaseResult<CharacteristicPatternDto>() { Data = FromModelToDto(model) };
        }

        private CharacteristicPatternDto FromModelToDto(CharacteristicPattern model)
        {
            return new CharacteristicPatternDto(model.Name);
        }

        public async Task<CollectionResult<CharacteristicPatternDto>> FromModelsToDtosAsync(List<CharacteristicPattern> models)
        {
            List<CharacteristicPatternDto> mappedCharacteristis = new List<CharacteristicPatternDto>();

            await Task.Run(() => 
            {
                foreach (CharacteristicPattern model in models)
                {
                    mappedCharacteristis.Add(FromModelToDto(model));
                }
            });

            return new CollectionResult<CharacteristicPatternDto>() { Count = mappedCharacteristis.Count, Data = mappedCharacteristis };
        }
    }
}
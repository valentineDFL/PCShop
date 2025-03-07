using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto;
using Domain.Dto.ProductDtos;
using Domain.Dto.CharacteristicPatternDto;

namespace Domain.Dto.CategoryDtos
{
    public record CategoryResponseDto(string Name, List<CharacteristicPatternDto.CharacteristicPatternDto> CharacteristicsPatterns);
}
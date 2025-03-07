using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.CategoryDto;
using Domain.Dto.CategoryDtos;
using Domain.Dto.CharacteristicRealization;

namespace Domain.Dto.ProductDtos
{
    public record ProductDto(string Name, string Description, decimal Price, float Weight, List<string> Categories, List<CharacteristicRealizationResponseDto> characterisitics);
}
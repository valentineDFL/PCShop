using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.CharacteristicPatternDto;
using Domain.Dto.ProductDtos;
using Domain.Models;

namespace Domain.Dto.CategoryDtos
{
    public record CreateCategoryDto(string Name, List<ProductDto> Products, List<CharacteristicPatternCreateDto> Characteristics);
}
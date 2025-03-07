using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.CategoryDtos;
using Domain.Models;

namespace Domain.Dto.ProductDtos
{
    public record CreateProductDto(string Name, string Description, decimal Price, float Weight, List<CategoryDto.CategoryDto> Categories,
     List<List<CharacteristicRealizationCreateDto>> characteristicsRealizations);


     // must have Category Patterns count * Categories Count
}
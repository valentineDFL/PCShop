using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto;
using Domain.Dto.ProductDtos;

namespace Domain.Dto.CategoryDtos
{
    public record CategoryDto(string Name, List<ProductDto> Products);
}
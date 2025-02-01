using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Dto.CategoryDtos
{
    public record CreateCategoryDto(string Name, List<Product> Products, List<Characteristic> Characteristics);
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Dto.ProductDtos
{
    public record UpdateProductDto(string Name, string Description, decimal Price, float Weight, List<Category> Categories);
}
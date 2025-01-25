using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto.ProductDtos
{
    public record ProductDto(string Name, string Description, decimal Price, float Weight, List<Guid> Categories);
}
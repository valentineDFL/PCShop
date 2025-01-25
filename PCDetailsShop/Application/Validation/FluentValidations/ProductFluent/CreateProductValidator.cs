using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.ProductDtos;
using FluentValidation;

namespace Application.Validation.FluentValidations.ProductFluent
{
    internal class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator() 
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.CategoryDtos;
using FluentValidation;

namespace Application.Validation.FluentValidations.CategoryFluent
{
    internal class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(c => c.Name).
                NotEmpty()
                .MaximumLength(32);
        }
    }
}

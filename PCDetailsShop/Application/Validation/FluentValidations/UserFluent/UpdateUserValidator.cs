using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.UserDtos;
using FluentValidation;

namespace Application.Validation.FluentValidations.UserFluent
{
    internal class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Login).NotEmpty().MaximumLength(32);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(32);
            RuleFor(x => x.OldPassword).NotEmpty().MaximumLength(32);
            RuleFor(x => x.NewPassword).NotEmpty().MaximumLength(32);
        }
    }
}

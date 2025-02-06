using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.UserDtos;
using Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;

namespace Application.Validation.FluentValidations.UserFluent
{
    internal class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Login).NotEmpty()
                .MaximumLength(32);

            RuleFor(x => x.Password).NotEmpty()
                .MaximumLength(32);

            RuleFor(x => x.Email).NotEmpty()
                .MaximumLength(32);

            RuleFor(x => x.BirthDate).NotEmpty()
                .Must(x => x.Year >= 18)
                .WithMessage("The User Must Be Over 16 Years Of Age");
        }
    }
}
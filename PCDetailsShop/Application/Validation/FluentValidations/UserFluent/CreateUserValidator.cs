using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.UserDtos;
using Domain.Enums;
using FluentValidation;

namespace Application.Validation.FluentValidations.UserFluent
{
    internal class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Login).NotEmpty()
                .Must(IllegalSymbols.NotContainsIllegalCharacter)
                .WithMessage(ValidationMessages.TurnedLoginContainIllegalCharacters)
                .MaximumLength(32);

            RuleFor(x => x.Password).NotEmpty()
                .MaximumLength(32)
                .Must(IllegalSymbols.NotContainsIllegalCharacter)
                .WithMessage(ValidationMessages.TurnedPasswordContainIllegalCharacters);

            RuleFor(x => x.Email).NotEmpty()
                .MaximumLength(32)
                .Must(IllegalSymbols.NotContainsIllegalCharacter)
                .WithMessage(ValidationMessages.TurnedEmailContainIllegalCharacters);

            RuleFor(x => x.BirthDate).NotEmpty()
                .Must(x => x.Year >= 18)
                .WithMessage(ValidationMessages.TheUserMustBeOver16YearsOfAge);
        }
    }
}
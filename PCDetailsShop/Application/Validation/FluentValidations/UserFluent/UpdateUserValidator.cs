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
    internal class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Login).NotEmpty()
                .MaximumLength(32)
                .Must(IllegalSymbols.ContainsIllegalCharacter)
                .WithMessage(ValidationMessages.TurnedLoginContainIllegalCharacters);

            RuleFor(x => x.Email).NotEmpty()
                .MaximumLength(32)
                .Must(IllegalSymbols.ContainsIllegalCharacter)
                .WithMessage(ValidationMessages.TurnedEmailContainIllegalCharacters);

            RuleFor(x => x.OldPassword).NotEmpty()
                .MaximumLength(32)
                .Must(IllegalSymbols.ContainsIllegalCharacter)
                .WithMessage(ValidationMessages.TurnedPasswordContainIllegalCharacters);

            RuleFor(x => x.NewPassword).NotEmpty()
                .MaximumLength(32)
                .Must(IllegalSymbols.ContainsIllegalCharacter)
                .WithMessage(ValidationMessages.TurnedNewPasswordContainIllegalCharacters);
        }
    }
}

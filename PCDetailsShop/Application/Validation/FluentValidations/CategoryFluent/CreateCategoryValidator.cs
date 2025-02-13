using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.CategoryDtos;
using Domain.Dto.CharacteristicPatternDto;
using FluentValidation;

namespace Application.Validation.FluentValidations.CategoryFluent
{
	internal class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
	{
		public CreateCategoryValidator()
		{
			RuleFor(c => c.Name).NotEmpty().MaximumLength(32);
			RuleFor(c => c.CharacteristicsToCreate).NotEmpty().Must(ctc => ctc.CheckRepeatContains() == false)
			.WithMessage("Category Characteristic Must non repeated");
		}
	}

	internal static class CharacteristicMethodExtensions
	{
		public static bool CheckRepeatContains(this List<CharacteristicPatternCreateDto> createCharacteristicDtos)
		{
			List<CharacteristicPatternCreateDto> notRepeatedCharacteristis = new List<CharacteristicPatternCreateDto>();
			
			foreach (CharacteristicPatternCreateDto dto in createCharacteristicDtos)
			{
				if(notRepeatedCharacteristis.Contains(dto))
					return true;
				else
					notRepeatedCharacteristis.Add(dto);
			}
			
			return false;
		}
	}
}
using System;
using System.Globalization;
using System.Threading.Tasks;
using Domain.Dto.CategoryDtos;
using Domain.Dto.CharacteristicPatternDto;
using Domain.Interfaces.Mapping;
using Domain.Models;
using Domain.Result;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PCDetailsShop.API.DtoMapping;

public class CategoryDtoMapper : ICategoryDtoMapper
{
    private readonly IDtoMapper<CharacteristicPattern, CharacteristicPatternDto> _characteristicPatternMapper;

    public CategoryDtoMapper(IDtoMapper<CharacteristicPattern, CharacteristicPatternDto> characteristicDtoMapper)
    {
        _characteristicPatternMapper = characteristicDtoMapper;
    }

    public async Task<BaseResult<CategoryResponseDto>> FromModelToDtoResultAsync(Category model)
    {
        CollectionResult<CharacteristicPatternDto> mappedCharacteristics = await _characteristicPatternMapper
        .FromModelsToDtosAsync(model.CharacteristicPatterns.ToList());

        CategoryResponseDto result = new CategoryResponseDto(model.Name, mappedCharacteristics.Data.ToList());

        return new BaseResult<CategoryResponseDto>() { Data = result };
    }

    private async Task<CategoryResponseDto> FromModelToDtoAsync(Category model)
    {
        CollectionResult<CharacteristicPatternDto> mappedCharacteristics = await _characteristicPatternMapper
            .FromModelsToDtosAsync(model.CharacteristicPatterns.ToList());

        return new CategoryResponseDto(model.Name, mappedCharacteristics.Data.ToList());
    }

    public async Task<CollectionResult<CategoryResponseDto>> FromModelsToDtosAsync(List<Category> models)
    {
        List<CategoryResponseDto> mappedCharacteristics = new List<CategoryResponseDto>();

        foreach(Category model in models)
        {
            mappedCharacteristics.Add(await FromModelToDtoAsync(model));
        }

        return new CollectionResult<CategoryResponseDto>() { Count = mappedCharacteristics.Count, Data = mappedCharacteristics };
    }
}
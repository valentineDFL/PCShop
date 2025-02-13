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

    public async Task<BaseResult<CategoryDto>> FromModelToDtoResultAsync(Category model)
    {
        CollectionResult<CharacteristicPatternDto> mappedCharacteristics = await _characteristicPatternMapper
        .FromModelsToDtosAsync(model.CharacteristicPatterns.ToList());

        CategoryDto result = new CategoryDto(model.Name, mappedCharacteristics.Data.ToList());

        return new BaseResult<CategoryDto>() { Data = result };
    }

    private async Task<CategoryDto> FromModelToDtoAsync(Category model)
    {
        CollectionResult<CharacteristicPatternDto> mappedCharacteristics = await _characteristicPatternMapper
            .FromModelsToDtosAsync(model.CharacteristicPatterns.ToList());

        return new CategoryDto(model.Name, mappedCharacteristics.Data.ToList());
    }

    public async Task<CollectionResult<CategoryDto>> FromModelsToDtosAsync(List<Category> models)
    {
        List<CategoryDto> mappedCharacteristics = new List<CategoryDto>();

        foreach(Category model in models)
        {
            mappedCharacteristics.Add(await FromModelToDtoAsync(model));
        }

        return new CollectionResult<CategoryDto>() { Count = mappedCharacteristics.Count, Data = mappedCharacteristics };
    }
}
using System;
using Domain.Dto.CategoryDtos;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Mapping;

public interface ICategoryDtoMapper
{
    public Task<BaseResult<CategoryResponseDto>> FromModelToDtoResultAsync(Category model);

    public Task<CollectionResult<CategoryResponseDto>> FromModelsToDtosAsync(List<Category> models);
}
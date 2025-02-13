using System;
using Domain.Dto.CategoryDtos;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Mapping;

public interface ICategoryDtoMapper
{
    public Task<BaseResult<CategoryDto>> FromModelToDtoResultAsync(Category model);

    public Task<CollectionResult<CategoryDto>> FromModelsToDtosAsync(List<Category> models);
}
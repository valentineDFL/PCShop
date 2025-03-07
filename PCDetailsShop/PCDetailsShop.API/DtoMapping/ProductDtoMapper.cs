using System;
using Domain.Dto.CharacteristicRealization;
using Domain.Dto.ProductDtos;
using Domain.Interfaces.Mapping;
using Domain.Models;
using Domain.Result;

namespace PCDetailsShop.API.DtoMapping;

public class ProductDtoMapper : IDtoMapper<Product, ProductDto>
{
    public BaseResult<ProductDto> FromModelToDtoResult(Product model)
    {
        List<string> categoriesNames = GetCategoriesNames(model.Categories.ToList());
        List<CharacteristicRealizationResponseDto> characteristics = GetCharacteristics(model.CharacteristicsRealizations);

        ProductDto result = new ProductDto(model.Name, model.Description, model.Price, model.Weight, categoriesNames, characteristics);

        return new BaseResult<ProductDto>() { Data = result }; 
    }

    private List<string> GetCategoriesNames(List<Category> categories)
    {
        List<string> categoriesNames = new List<string>();

        foreach(Category category in categories)
        {
            categoriesNames.Add(category.Name);
        }

        return categoriesNames;
    }

    private List<CharacteristicRealizationResponseDto> GetCharacteristics(List<CharacteristicRealization> realizations)
    {
        List<CharacteristicRealizationResponseDto> characteristics = new List<CharacteristicRealizationResponseDto>();

        foreach(CharacteristicRealization realization in realizations)
        {
            CharacteristicRealizationResponseDto characteristic = new CharacteristicRealizationResponseDto(realization.CharacteristicPattern.Name, realization.Value);
            characteristics.Add(characteristic);
        }

        return characteristics;
    }

    public async Task<CollectionResult<ProductDto>> FromModelsToDtosAsync(List<Product> models)
    {
        List<ProductDto> productDtos = new List<ProductDto>();

        await Task.Run(() => 
        {
            foreach(Product product in models)
            {
                productDtos.Add(FromModelToDtoResult(product).Data);
            }
        });

        return new CollectionResult<ProductDto>() { Count = productDtos.Count, Data = productDtos };
    }
}
using System;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using Domain.Dto;
using Domain.Dto.CategoryDto;
using Domain.Dto.ProductDtos;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Result;
using Microsoft.AspNetCore.Mvc.Formatters;
using Serilog;

namespace Application.Services;

public class ProductService : IProductService
{
	private readonly IProductRepository _productRepository;
	private readonly ICharacteristicRealizationRepository _characteristicRealizationRepository;

	private readonly ICartRepository _cartRepository;
	private readonly ICategoryRepository _categoryRepository;
	
	public ProductService(IProductRepository productRepository, ICharacteristicRealizationRepository characteristicRealizeRepository, 
	ICartRepository cartRepository, ICategoryRepository categoryRepository)
	{
		_productRepository = productRepository;
		_characteristicRealizationRepository = characteristicRealizeRepository;
		_cartRepository = cartRepository;
		_categoryRepository = categoryRepository;
	}
	
	public async Task<CollectionResult<Product>> GetAllAsync()
	{
		List<Product> products = await _productRepository.GetAllAsync();
		
		if(products.Count == 0)
		{
			return new CollectionResult<Product>()
			{
				ErrorCode = (int)ErrorCodes.ProductsNotFound,
				ErrorMessage = ErrorCodes.ProductsNotFound.ToString()
			};
		}

		return new CollectionResult<Product>() { Count = products.Count, Data = products };
	}
	
	public async Task<BaseResult<Product>> GetByIdAsync(Guid id)
	{
		(Product product, ErrorCodes errorCode) findedProduct = await _productRepository.GetByIdAsync(id);

		if(findedProduct.errorCode != ErrorCodes.None)
		{
			return new BaseResult<Product>() 
			{ 
				ErrorCode = (int)ErrorCodes.ProductNotFound,
				ErrorMessage = ErrorCodes.ProductsNotFound.ToString() 
			};			
		}

		return new BaseResult<Product>() { Data = findedProduct.product };
	}
	
	public async Task<CollectionResult<Product>> GetByNamePartAsync(string namePart)
	{
		List<Product> findedProducts = await _productRepository.GetByNamePartAsync(namePart);

		if(findedProducts.Count == 0)
		{
			return new CollectionResult<Product>() 
			{ 
				ErrorCode = (int)ErrorCodes.ProductNotFound,
				ErrorMessage = ErrorCodes.ProductsNotFound.ToString() 
			};			
		}

		return new CollectionResult<Product>() { Count = findedProducts.Count, Data = findedProducts };
	}
	
	public async Task<CollectionResult<Product>> GetByCategoriesNamesAsync(string[] categories, int countPerPage, int page)
	{
		List<Product> findedProducts = await _productRepository.GetByCategoriesNamesAsync(categories, countPerPage, page);

		if(findedProducts.Count == 0)
		{
			return new CollectionResult<Product>()
			{
				ErrorCode = (int)ErrorCodes.ProductsNotFound,
				ErrorMessage = ErrorCodes.ProductsNotFound.ToString(),
			};
		}

		return new CollectionResult<Product>() { Count = findedProducts.Count, Data = findedProducts }; 
	}
	
	public async Task<BaseResult<Product>> CreateAsync(CreateProductDto dto)
	{
		if(dto.Price < 0)
		{
			return new BaseResult<Product>()
			{
				ErrorCode = (int)ErrorCodes.ProductPriceMustBeGreaterThanZero,
				ErrorMessage = ErrorCodes.ProductPriceMustBeGreaterThanZero.ToString()
			};
		}

        if (dto.Weight < 0)
        {
            return new BaseResult<Product>()
            {
                ErrorCode = (int)ErrorCodes.ProductWeightMustBeGreaterThanZero,
                ErrorMessage = ErrorCodes.ProductWeightMustBeGreaterThanZero.ToString()
            };
        }

        (Product product, ErrorCodes errorCode) findedProduct = await _productRepository.GetByNameAsync(dto.Name);

		if(findedProduct.errorCode == ErrorCodes.None)
		{
			return new BaseResult<Product>()
			{
				ErrorCode = (int)ErrorCodes.ProductAlreadyExists,
				ErrorMessage = ErrorCodes.ProductAlreadyExists.ToString()
			};
		}

		CollectionResult<Category> findedCategories = await FindTurnedProductCategoriesAsync(dto.Categories);

		if(!findedCategories.IsSuccess)
		{
			return new BaseResult<Product>()
			{
				ErrorCode = findedCategories.ErrorCode,
				ErrorMessage = findedCategories.ErrorMessage
			};
		}

        if (findedCategories.Count == 0)
			return await CreateProductAsync(dto.Name, dto.Description, dto.Price, dto.Weight, findedCategories.Data, new List<CharacteristicRealization>());

			CollectionResult<CharacteristicRealization> characteristicRealizations = 
							CreateProductCharacteristicRealizations(dto.characteristicsRealizations, findedCategories.Data, findedProduct.product);

			if(!characteristicRealizations.IsSuccess)
			{
				return new BaseResult<Product>()
				{
					ErrorCode = characteristicRealizations.ErrorCode,
					ErrorMessage = characteristicRealizations.ErrorMessage,
				};
			}

			return await CreateProductAsync(dto.Name, dto.Description, dto.Price, dto.Weight, findedCategories.Data, characteristicRealizations.Data);
	}

	private async Task<BaseResult<Product>> CreateProductAsync(string name, string description, decimal price, float weight, List<Category> categories, List<CharacteristicRealization> realizations)
	{
        Guid productId = Guid.NewGuid();

        Product productToCreate = new Product(productId, name, description, price, weight, true, 0, categories, realizations);

        Product createdProduct = await _productRepository.CreateAsync(productToCreate);

        return new BaseResult<Product>() { Data = createdProduct };
    }

	private async Task<CollectionResult<Category>> FindTurnedProductCategoriesAsync(List<CategoryDto> categoriesDtos)
	{
		List<string> names = new List<string>();
		foreach (var categoryDto in categoriesDtos)
		{
			names.Add(categoryDto.Name);
		}

	    List<Category> findedCategoriesByName = await _categoryRepository.GetByNamesAsync(names);

        return new CollectionResult<Category>() { Count = findedCategoriesByName.Count, Data = findedCategoriesByName };
	}

	private CollectionResult<CharacteristicRealization> CreateProductCharacteristicRealizations(List<List<CharacteristicRealizationCreateDto>> realizationsToCreate, List<Category> categories, Product product)
	{
		if(realizationsToCreate.Count != categories.Count)
		{
			return new CollectionResult<CharacteristicRealization>()
			{
				ErrorCode = (int)ErrorCodes.TheNumberOfImplementationsDoesNotMatchTheNumberOfTemplates,
				ErrorMessage = ErrorCodes.TheNumberOfImplementationsDoesNotMatchTheNumberOfTemplates.ToString(),
			};
		}
		List<CharacteristicRealization> characteristicRealizations = new List<CharacteristicRealization>();

		for(int i = 0; i < realizationsToCreate.Count; i++)
		{
			if(realizationsToCreate[i].Count != categories[i].CharacteristicPatterns.Count)
			{
				return new CollectionResult<CharacteristicRealization>()
				{
					ErrorCode = (int)ErrorCodes.TheNumberOfRealizationsOfTheCategoryPatternsDoesNotCorrespondToTheRequiredNumberOfPatterns,
					ErrorMessage = ErrorCodes.TheNumberOfRealizationsOfTheCategoryPatternsDoesNotCorrespondToTheRequiredNumberOfPatterns.ToString(),
				};
			}

			for (int j = 0; j < realizationsToCreate[i].Count; j++)
			{
				Guid realizationId = Guid.NewGuid();
				string value = realizationsToCreate[i][j].Value;
				Guid patternId = categories[i].CharacteristicPatterns[j].Id;
				CharacteristicPattern pattern = categories[i].CharacteristicPatterns[j];

				CharacteristicRealization realization = new CharacteristicRealization(realizationId, value, patternId, pattern, product.Id, product);
				characteristicRealizations.Add(realization);
			}
		}

		return new CollectionResult<CharacteristicRealization>() { Count = characteristicRealizations.Count, Data = characteristicRealizations };
	}
	
	public async Task<BaseResult<Guid>> DeleteByIdAsync(Guid productId)
	{
		(Product product, ErrorCodes errorCode) findedProduct = await _productRepository.GetByIdAsync(productId);

		if(findedProduct.errorCode != ErrorCodes.None)
		{
			return new BaseResult<Guid>()
			{
				ErrorCode = (int)ErrorCodes.ProductNotFound,
				ErrorMessage = ErrorCodes.ProductNotFound.ToString(),
			};
		}

		int deletedProducts = await _productRepository.DeleteByIdAsync(productId);

		if(deletedProducts == 0)
		{
            return new BaseResult<Guid>()
            {
                ErrorCode = (int)ErrorCodes.InternalServerError,
                ErrorMessage = ErrorCodes.InternalServerError.ToString(),
            };
        }

		return new BaseResult<Guid>() { Data = productId };
	}
	
	public async Task<BaseResult<string>> ChangeNameByIdAsync(Guid productId, string newName)
	{
		(Product product, ErrorCodes errorCode) findedProduct = await _productRepository.GetByNameAsync(newName);

		if(findedProduct.errorCode == ErrorCodes.None)
		{
			return new BaseResult<string>()
			{
				ErrorCode = (int)ErrorCodes.ProductAlreadyExists,
				ErrorMessage = ErrorCodes.ProductAlreadyExists.ToString(),
			};
		}

		int updatedProducts = await _productRepository.ChangeNameAsync(productId, newName);

		if(updatedProducts == 0)
		{
			return new BaseResult<string>()
			{
				ErrorCode = (int)ErrorCodes.ProductNotFound,
				ErrorMessage = ErrorCodes.ProductNotFound.ToString()
			};
		}

		return new BaseResult<string>() { Data = newName };
	}
	
	public async Task<BaseResult<string>> ChangeDescriptionByIdAsync(Guid productId, string newDescription)
	{
		int updatedProducts = await _productRepository.ChangeDescriptionAsync(productId, newDescription);

		if(updatedProducts == 0)
		{
			return new BaseResult<string>()
			{
				ErrorCode = (int)ErrorCodes.ProductNotFound,
				ErrorMessage = ErrorCodes.ProductNotFound.ToString(),
			};
		}

		return new BaseResult<string>() { Data = newDescription };
	}

	public async Task<BaseResult<decimal>> ChangePriceByIdAsync(Guid productId, decimal newPrice)
	{
		if(newPrice <= 0)
		{
			return new BaseResult<decimal>()
			{
				ErrorCode = (int)ErrorCodes.ProductPriceMustBeGreaterThanZero,
				ErrorMessage = ErrorCodes.ProductPriceMustBeGreaterThanZero.ToString(),
			};
		}

        int updatedProducts = await _productRepository.ChangePriceAsync(productId, newPrice);

		if(updatedProducts == 0)
		{
			return new BaseResult<decimal>()
			{
				ErrorCode = (int)ErrorCodes.ProductNotFound,
				ErrorMessage = ErrorCodes.ProductNotFound.ToString(),
			};
		}
        return new BaseResult<decimal>() { Data = newPrice };
	}

	public async Task<BaseResult<float>> ChangeWeightByIdAsync(Guid productId, float newWeight)
	{
		if(newWeight <= 0)
		{
			return new BaseResult<float>()
			{
				ErrorCode = (int)ErrorCodes.ProductWeightMustBeGreaterThanZero,
				ErrorMessage = ErrorCodes.ProductWeightMustBeGreaterThanZero.ToString(),
			};
		}

		int updatedProducts = await _productRepository.ChangeWeightAsync(productId, newWeight);

		if(updatedProducts == 0)
		{
			return new BaseResult<float>()
			{
				ErrorCode = (int)ErrorCodes.ProductNotFound,
				ErrorMessage = ErrorCodes.ProductNotFound.ToString(),
			};
		}

		return new BaseResult<float>() { Data = newWeight };
	}

	public async Task<BaseResult<int>> ChangeProductCountAsync(Guid productId, int newCount)
	{
		if(newCount < 0)
		{
			return new BaseResult<int>()
			{
				ErrorCode = (int)ErrorCodes.NewProductCountMustBeGraaterThanZero,
				ErrorMessage = ErrorCodes.NewProductCountMustBeGraaterThanZero.ToString(),
			};
		}

		(Product product, ErrorCodes errorCode) findedProduct = await _productRepository.GetByIdAsync(productId);

		if(findedProduct.errorCode != ErrorCodes.None)
		{
			return new BaseResult<int>()
			{
				ErrorCode = (int)findedProduct.errorCode,
				ErrorMessage = findedProduct.errorCode.ToString()
			};
		}

		await _productRepository.ChangeProductCountAsync(productId, newCount);

		return new BaseResult<int>() { Data = newCount };
	}

	public async Task<BaseResult<string>> AddCategoryToProductByNameAsync(Guid productId, string categoryName, List<CharacteristicRealizationCreateDto> categoryCharacteristicRealizations)
	{
		var findedCategory = await _categoryRepository.GetByNameAsync(categoryName);

		if(findedCategory.errorCode != ErrorCodes.None)
		{
			return new BaseResult<string>()
			{
				ErrorCode = (int)findedCategory.errorCode,
				ErrorMessage = findedCategory.errorCode.ToString(),
			};
		}

        if (categoryCharacteristicRealizations.Count != findedCategory.category.CharacteristicPatterns.Count)
		{
			return new BaseResult<string>()
			{
				ErrorCode = (int)ErrorCodes.TheNumberOfImplementationsDoesNotMatchTheNumberOfTemplates,
				ErrorMessage = ErrorCodes.TheNumberOfImplementationsDoesNotMatchTheNumberOfTemplates.ToString()
			};
		}

		var findedProduct = await _productRepository.GetByIdAsync(productId);

		if (findedProduct.errorCode != ErrorCodes.None)
		{
			return new BaseResult<string>()
			{
				ErrorCode = (int)ErrorCodes.ProductNotFound,
				ErrorMessage = ErrorCodes.ProductNotFound.ToString(),
			};
		}

        if (findedProduct.product.Categories.Any(cat => cat.Name == findedCategory.category.Name))
		{
			return new BaseResult<string>()
			{
				ErrorCode = (int)ErrorCodes.ProductContainTurnedCategory,
				ErrorMessage = ErrorCodes.ProductContainTurnedCategory.ToString()
			};
		}

        var createdRealizations = CreateRealizations(categoryCharacteristicRealizations, findedCategory.category, findedProduct.product);

		var addedCategory = await _productRepository.AddCategoryToProductAsync(findedProduct.product, findedCategory.category, createdRealizations);
		var addedRealizations = await _characteristicRealizationRepository.CreateRangeAsync(createdRealizations);

		if(addedRealizations.Count == 0)
		{
			return new BaseResult<string>()
			{
				ErrorCode = (int)ErrorCodes.CartNotFound,
				ErrorMessage = ErrorCodes.CartNotFound.ToString(),
			};
		}

		if(addedCategory.errorCode != ErrorCodes.None)
		{
			return new BaseResult<string>()
			{
				ErrorCode = (int)addedCategory.errorCode,
				ErrorMessage = addedCategory.errorCode.ToString(),
			};
		}

		return new BaseResult<string>() { Data = addedCategory.category.Name };
	}
	
	private List<CharacteristicRealization> CreateRealizations(List<CharacteristicRealizationCreateDto> createRealizationDtos, Category category, Product product)
	{
		List<CharacteristicRealization> createdCategoryRealizations = new List<CharacteristicRealization>();

		for(int index = 0; index < category.CharacteristicPatterns.Count; index++)
		{
			Guid realizationId = Guid.NewGuid();
			string value = createRealizationDtos[index].Value;
			Guid characteristicId = category.CharacteristicPatterns[index].Id;
			CharacteristicPattern characteristicPattern = category.CharacteristicPatterns[index];

			CharacteristicRealization createdRealization = new CharacteristicRealization(realizationId, value, characteristicId, null, product.Id, null);
			createdCategoryRealizations.Add(createdRealization);
		}

		return createdCategoryRealizations;
	}

	public async Task<BaseResult<string>> DeleteCategoryFromProductByNameAsync(Guid productId, string categoryName)
	{
		var findedCategory = await _categoryRepository.GetByNameAsync(categoryName);

		if(findedCategory.errorCode != ErrorCodes.None)
		{
			return new BaseResult<string>()
			{
				ErrorCode = (int)ErrorCodes.CategoryNotFound,
				ErrorMessage = ErrorCodes.CategoriesNotFound.ToString()
			};
		}

		var removedCategory = await _productRepository.RemoveCategoryFromProduct(productId, findedCategory.category);

		if(removedCategory.errorCode != ErrorCodes.None)
		{
			return new BaseResult<string>()
			{
				ErrorCode = (int)removedCategory.errorCode,
				ErrorMessage = removedCategory.errorCode.ToString()
			};
		}

		return new BaseResult<string>() { Data = findedCategory.category.Name };
	}

	public async Task<BaseResult<string>> AddProductToCartAsync(Guid productId, Guid cartId)
	{
		var addedProduct = await _cartRepository.AddProductAsync(cartId, productId);

		if(addedProduct.errorCode != ErrorCodes.None)
		{
			return new BaseResult<string>() { ErrorCode = (int)addedProduct.errorCode, ErrorMessage = addedProduct.errorCode.ToString() };
		}

		return new BaseResult<string>() { Data = addedProduct.product.Name };
	}

	public async Task<BaseResult<string>> ChangeCharacteristicValueAsync(Guid productId, string characteristicName, string newCharacteristicValue)
	{
		(Product product, ErrorCodes errorCode) findedProduct = await _productRepository.GetByIdAsync(productId);

		if(findedProduct.errorCode != ErrorCodes.None)
		{
			return new BaseResult<string>()
			{
				ErrorCode = (int)ErrorCodes.ProductNotFound,
				ErrorMessage = ErrorCodes.ProductNotFound.ToString()
			};
		}

		CharacteristicRealization productRealization = findedProduct.product.CharacteristicsRealizations
				.FirstOrDefault(c => c.CharacteristicPattern.Name == characteristicName);

		if(productRealization == null)
		{
			return new BaseResult<string>()
			{
				ErrorCode = (int)ErrorCodes.CharacteristicRealizationNotFound,
				ErrorMessage = ErrorCodes.CharacteristicRealizationNotFound.ToString()
			};
		}

		await _characteristicRealizationRepository.ChangeRealizationValueByIdAsync(productRealization.Id, newCharacteristicValue);

        return new BaseResult<string>() { Data = newCharacteristicValue };
	}
}
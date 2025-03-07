using System.Dynamic;
using Domain.Dto;
using Domain.Dto.CategoryDto;
using Domain.Dto.ProductDtos;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Services
{
	public interface IProductService
	{
		public Task<CollectionResult<Product>> GetAllAsync();

		public Task<BaseResult<Product>> GetByIdAsync(Guid id);

		public Task<CollectionResult<Product>> GetByNamePartAsync(string namePart);
		
		public Task<CollectionResult<Product>> GetByCategoriesNamesAsync(string[] categories, int countPerPage, int page);

		public Task<BaseResult<Product>> CreateAsync(CreateProductDto dto);

		public Task<BaseResult<Guid>> DeleteByIdAsync(Guid id);

		public Task<BaseResult<string>> ChangeNameByIdAsync(Guid productId, string newName);

		public Task<BaseResult<string>> ChangeDescriptionByIdAsync(Guid productId, string newDescription);

		public Task<BaseResult<decimal>> ChangePriceByIdAsync(Guid productId, decimal newPrice);

		public Task<BaseResult<float>> ChangeWeightByIdAsync(Guid productId, float newWeight);

		public Task<BaseResult<int>> ChangeProductCountAsync(Guid productId, int newCount);

        public Task<BaseResult<string>> AddCategoryToProductByNameAsync(Guid productId, string categoryName, List<CharacteristicRealizationCreateDto> categoryCharacteristicRealizations);

		public Task<BaseResult<string>> DeleteCategoryFromProductByNameAsync(Guid productId, string categoryName);

		public Task<BaseResult<string>> AddProductToCartAsync(Guid productId, Guid cartId);

		public Task<BaseResult<string>> ChangeCharacteristicValueAsync(Guid productId, string characteristicName, string newCharacteristicValue);
	} 
}
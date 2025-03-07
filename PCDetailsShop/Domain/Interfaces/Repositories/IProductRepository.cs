using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.CategoryDto;
using Domain.Enums;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Repositories
{
	public interface IProductRepository
	{
		public Task<List<Product>> GetAllAsync();

		public Task<(Product product, ErrorCodes errorCode)> GetByIdAsync(Guid id);

		public Task<List<Product>> GetByNamePartAsync(string namePart);

		public Task<(Product product, ErrorCodes errorCode)> GetByNameAsync(string name);

        public Task<List<Product>> GetByCategoriesNamesAsync(string[] categories, int countPerPage, int page);

		public Task<Product> CreateAsync(Product product);

        public Task<int> DeleteByIdAsync(Guid id);

		public Task<int> ChangeNameAsync(Guid id, string newName);

		public Task<int> ChangeDescriptionAsync(Guid id, string description);

        public Task<int> ChangePriceAsync(Guid id, decimal newPrice);

        public Task<int> ChangeWeightAsync(Guid id, float newWeight);

        public Task<int> ChangeProductCountAsync(Guid id, int newCount);

		public Task<(Category category, ErrorCodes errorCode)> AddCategoryToProductAsync(Product product, Category categoryToAdd, List<CharacteristicRealization> realizations);

		public Task<(Category category, ErrorCodes errorCode)> RemoveCategoryFromProduct(Guid productId, Category categoryToRemove);
    }
}
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Xml;
using Domain.Dto.CategoryDto;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Result;
using Microsoft.EntityFrameworkCore;
using Npgsql.Replication.PgOutput;

namespace DataAccessLayer.Repositories
{
	internal class ProductRepository : IProductRepository
	{
		private readonly PcShopDbContext _dbContext;

		public ProductRepository(PcShopDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Product>> GetAllAsync()
        {
            List<Product> entities = await _dbContext.Products
				.AsNoTracking()
                .Include(p => p.Categories)
				.ThenInclude(c => c.CharacteristicPatterns)
				.Include(p => p.CharacteristicsRealizations)
				.ThenInclude(cr => cr.CharacteristicPattern)
                .ToListAsync();

			if(entities.Count == 0)
				return new List<Product>();

			return entities;
        }

		public async Task<(Product product, ErrorCodes errorCode)> GetByIdAsync(Guid id)
        {
            Product entity = await _dbContext.Products
					.Where(p => p.Id == id)
                    .AsNoTracking()
                    .Include(p => p.Categories)
					.ThenInclude(c => c.CharacteristicPatterns)
					.Include(p => p.CharacteristicsRealizations)
					.ThenInclude(cr => cr.CharacteristicPattern)
					.FirstOrDefaultAsync();

			if(entity == null)
				return (null, ErrorCodes.ProductNotFound);

			return (entity, ErrorCodes.None);
        }

		public async Task<List<Product>> GetByNamePartAsync(string namePart)
        {
            List<Product> entities = await _dbContext.Products
						.Where(p => p.Name.Contains(namePart))
                        .AsNoTracking()
                        .Include(p => p.Categories)
						.ThenInclude(c => c.CharacteristicPatterns)
						.Include(p => p.CharacteristicsRealizations)
						.ThenInclude(cr => cr.CharacteristicPattern)
                        .ToListAsync();

			if(entities.Count == 0)
				return new List<Product>();

			return entities;
        }
		
		public async Task<(Product product, ErrorCodes errorCode)> GetByNameAsync(string name)
		{
			Product productEntity = await _dbContext.Products
					.AsNoTracking()
                    .Include(p => p.Categories)
                    .ThenInclude(c => c.CharacteristicPatterns)
                    .Include(p => p.CharacteristicsRealizations)
                    .ThenInclude(cr => cr.CharacteristicPattern)
                    .FirstOrDefaultAsync(p => p.Name == name);

			if (productEntity == null)
				return (null, ErrorCodes.ProductNotFound);

			return (productEntity, ErrorCodes.None);
        }

		public async Task<List<Product>> GetByCategoriesNamesAsync(string[] categoryDto, int countPerPage, int page)
        {
			List<Product> entities = await _dbContext.Products
						.Where(p => p.Categories.All(cat => categoryDto.Contains(cat.Name))) // ce => categoryDto.Any(catd => catd.Name == ce.Name)
                        .AsNoTracking()
                        .Include(p => p.Categories)
						.ThenInclude(c => c.CharacteristicPatterns)
						.Include(p => p.CharacteristicsRealizations)
						.ThenInclude(cr => cr.CharacteristicPattern)
                        .ToListAsync();

			if(entities.Count == 0)
				return new List<Product>();

			List<Product> findedEntities = entities.Skip(countPerPage * page).Take(countPerPage * page + 1).ToList();

			if (findedEntities.Count == 0)
				return new List<Product>();

			return entities;
		}

        public async Task<Product> CreateAsync(Product product)
        {
			foreach (var entity in product.Categories)
			{
				_dbContext.Entry(entity).State = EntityState.Unchanged;
			}

			List<CharacteristicPattern> patterns = product.CharacteristicsRealizations
								.Select(x => x.CharacteristicPattern)
								.ToList();

			foreach (var entity in patterns)
			{
				_dbContext.Entry(entity).State = EntityState.Unchanged;
			}

			await _dbContext.Products.AddAsync(product);
			await _dbContext.SaveChangesAsync();

			return product;
        }

        public async Task<int> DeleteByIdAsync(Guid id)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync()) 
			{
				try
				{
					Product product = await _dbContext.Products
						.Where(p => p.Id == id)
						.Include(p => p.CharacteristicsRealizations)
						.FirstOrDefaultAsync();

					List<Guid> characteristicToDeleteId = product.CharacteristicsRealizations
							.Select(x => x.Id)
							.ToList();

                    int deletedCharacteristics = await _dbContext.CharacteristicRealizations
						.Where(ch => characteristicToDeleteId.Contains(ch.Id))
						.ExecuteDeleteAsync();

                    int deletedProducts = await _dbContext.Products
						.Where(p => p.Id == product.Id)
						.ExecuteDeleteAsync();

					await _dbContext.SaveChangesAsync();
					await transaction.CommitAsync();

					return deletedProducts;
				}
				catch(Exception ex)
				{
                    Console.WriteLine(ex.Message);
					await transaction.RollbackAsync();
					return 0;
				}
			}
        }

        public async Task<int> ChangeNameAsync(Guid id, string newName)
		{
			int updatedProducts = await _dbContext.Products
				.Where(p => p.Id == id)
				.ExecuteUpdateAsync(p => p
				.SetProperty(p => p.Name, newName));

			await _dbContext.SaveChangesAsync();

			return updatedProducts;
		}

		public async Task<int> ChangeDescriptionAsync(Guid id, string description)
		{
            int updatedProducts = await _dbContext.Products
				.Where(p => p.Id == id)
				.ExecuteUpdateAsync(p => p
				.SetProperty(p => p.Description, description));

            await _dbContext.SaveChangesAsync();

            return updatedProducts;
        }

        public async Task<int> ChangePriceAsync(Guid id, decimal newPrice)
        {
            int updatedProducts = await _dbContext.Products
				.Where(p => p.Id == id)
				.ExecuteUpdateAsync(p => p
				.SetProperty(p => p.Price, newPrice));

            await _dbContext.SaveChangesAsync();

            return updatedProducts;
        }

        public async Task<int> ChangeWeightAsync(Guid id, float newWeight)
        {
            int updatedProducts = await _dbContext.Products
				.Where(p => p.Id == id)
				.ExecuteUpdateAsync(p => p
				.SetProperty(p => p.Weight, newWeight));

            await _dbContext.SaveChangesAsync();

            return updatedProducts;
        }

		public async Task<int> ChangeProductCountAsync(Guid id, int newCount)
		{
			int updatedProducts = await _dbContext.Products
				.Where(p => p.Id == id)
				.ExecuteUpdateAsync(p => p
				.SetProperty(p => p.Count, newCount));

			return updatedProducts;
		}

		public async Task<(Category category, ErrorCodes errorCode)> AddCategoryToProductAsync(Product product, Category categoryToAdd, List<CharacteristicRealization> realizations)
		{
				_dbContext.Products.Attach(product);
				product.Categories.Add(categoryToAdd);

				// product.CharacteristicsRealizations.AddRange(realizations);

				await _dbContext.SaveChangesAsync();

				return (categoryToAdd, ErrorCodes.None);
		}

		public async Task<(Category category, ErrorCodes errorCode)> RemoveCategoryFromProduct(Guid productId, Category categoryToRemove)
		{
            Product productEntity = await _dbContext.Products
				.Include(p => p.Categories)
				.Include(p => p.CharacteristicsRealizations)
				.ThenInclude(p => p.CharacteristicPattern)
				.FirstOrDefaultAsync(p => p.Id == productId);

            if (productEntity == null)
                return (null, ErrorCodes.ProductNotFound);

            if (!productEntity.Categories.Any(cat => cat.Name == categoryToRemove.Name))
                return (null, ErrorCodes.ProductDontContainTurnedCategory);

            var category = productEntity.Categories.FirstOrDefault(c => c.Id == categoryToRemove.Id);
			productEntity.Categories.Remove(category);

			List<Guid> characteristicsToRemoveIds = categoryToRemove.CharacteristicPatterns.Select(x => x.Id).ToList();

            List<CharacteristicRealization> characteristicsToRemove = productEntity.CharacteristicsRealizations
								.Where(cr => characteristicsToRemoveIds.Contains(cr.CharacteristicPatternId))
								.ToList();

            foreach(var realization in characteristicsToRemove)
			{
				productEntity.CharacteristicsRealizations.Remove(realization);
			}

			await _dbContext.CharacteristicRealizations
				.Where(cr => characteristicsToRemoveIds.Contains(cr.CharacteristicPatternId))
				.ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();

            return (categoryToRemove, ErrorCodes.None);
		}
	}
}
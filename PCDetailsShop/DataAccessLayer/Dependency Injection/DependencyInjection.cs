using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Repositories;
using Domain.Interfaces.Repositories;

namespace DataAccessLayer.Dependency_Injection
{
	public static class DependencyInjection
	{
		public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
		{
			string connectionString = configuration.GetConnectionString("PostgresSQL");

			services.AddDbContext<PcShopDbContext>(option => option.UseNpgsql(connectionString));

			InitRepositories(services);
		}

		private static void InitRepositories(this IServiceCollection services)
		{
			services.AddScoped<IUserRepository, UserRepository>();

			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<ICartRepository, CartRepository>();
			services.AddScoped<ICharacteristicPatternRepository, CharacteristicPatternRepository>();
			services.AddScoped<ICharacteristicRealizationRepository, CharacteristicRealizationRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
		}
	}
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Repositories;
using Domain.Interfaces.Repositories;
using DataAccessLayer.Entities;
using Domain.Models;
using DataAccessLayer.Mapping;

namespace DataAccessLayer.Dependency_Injection
{
	public static class DependencyInjection
	{
		public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
		{
			string connectionString = configuration.GetConnectionString("PostgresSQL");

			services.AddDbContext<PcShopDbContext>(option => option.UseNpgsql(connectionString));

			InitMapping(services);
			InitRepositories(services);
		}

		private static void InitRepositories(this IServiceCollection services)
		{
			services.AddScoped<IUserRepository, UserRepository>();

			services.AddScoped<ICartRepository, CartRepository>();
			services.AddScoped<ICharacteristicPatternRepository, CharacteristicPatternRepository>();
			//services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
		}

		private static void InitMapping(this IServiceCollection services)
		{
			services.AddScoped(typeof(UserMapper));
			services.AddScoped(typeof(CharacteristicPatternMapper));
			services.AddScoped(typeof(CharacteristicRealizationMapper));
			services.AddScoped(typeof(CategoryMapper));
			services.AddScoped(typeof(ProductMapper));
			services.AddScoped(typeof(CartMapper));
		}
	}
}
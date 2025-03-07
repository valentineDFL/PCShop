using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Domain.Interfaces.Validators;
using Serilog;
using FluentValidation;
using Domain.Dto.UserDtos;
using Domain.Interfaces.Encrypt;
using Application.Encrypters;
using Application.Validation.FluentValidations.UserFluent;
using Domain.Dto.CategoryDtos;
using Application.Validation.FluentValidations.CategoryFluent;

namespace Application.DependencyInjection
{
	public static class DependencyInjection
	{
		public static void AddApplication(this IServiceCollection services)
		{
			services.AddScoped<IEncrypter, ShaEncrypter>();

			services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();
			services.AddScoped<IValidator<CreateCategoryDto>, CreateCategoryValidator>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<ICartService, CartService>();
		}
	}
}

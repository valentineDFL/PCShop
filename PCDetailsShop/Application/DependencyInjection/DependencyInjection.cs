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
using Application.Validation.UserValidations;
using FluentValidation;
using Domain.Dto.UserDtos;
using Domain.Interfaces.Encrypt;
using Application.Encrypters;
using Application.Validation.FluentValidations.UserFluent;
using Domain.Dto.CategoryDtos;

namespace Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IEncrypter, ShaEncrypter>();

            InitUserService(services);
            //InitCartService(services);
            //InitProductService(services);
            //InitCategoryService(services);

        }

        private static void InitUserService(this IServiceCollection services)
        {
            services.AddScoped<IUserValidator, UserValidator>();
            services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();

            services.AddScoped<IUserService, UserService>();
        }

        private static void InitCartService(this IServiceCollection services)
        {

        }

        private static void InitProductService(this IServiceCollection services)
        {

        }

        private static void InitCategoryService(this IServiceCollection services)
        {
            
        }
    }
}

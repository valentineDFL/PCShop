using System.Runtime.CompilerServices;
using System.Text;
using Application.Jwt;
using Domain.Dto.CharacteristicPatternDto;
using Domain.Dto.ProductDtos;
using Domain.Dto.UserDtos;
using Domain.Enums;
using Domain.Interfaces.Auth;
using Domain.Interfaces.Mapping;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PCDetailsShop.API.DtoMapping;
using PCDetailsShop.API.Permissions;

namespace PCDetailsShop.API.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddDtoMapping(this IServiceCollection services)
        {
            services.AddScoped<IDtoMapper<Product, ProductDto>, ProductDtoMapper>();
            services.AddScoped<IDtoMapper<User, UserDto>, UserDtoMapper>();
            services.AddScoped<IDtoMapper<CharacteristicPattern, CharacteristicPatternDto>, CharacteristicPatternDtoMapper>();
            services.AddScoped<ICategoryDtoMapper, CategoryDtoMapper>();
            services.AddScoped<IJwtProvider, JwtProvider>();
        }

        public static void AddApiAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateActor = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies[CookiesCodes.AuthCookie];
                            Console.WriteLine("Я ВЫЗЫВАЮСЬ ЧАСТО");

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddAuthorization(option =>
            {
                option.AddPolicy(nameof(Roles.Admin), policy =>
                {
                    policy.Requirements.Add(new PermissionsRequirment([Domain.Enums.Permissions.Read]));
                });
            });
        }
    }
}
using System.Runtime.CompilerServices;
using Domain.Dto.CharacteristicPatternDto;
using Domain.Dto.UserDtos;
using Domain.Interfaces.Mapping;
using Domain.Models;
using PCDetailsShop.API.DtoMapping;

namespace PCDetailsShop.API.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddDtoMapping(this IServiceCollection services)
        {
            services.AddScoped<IDtoMapper<User, UserDto>, UserDtoMapper>();
            services.AddScoped<IDtoMapper<CharacteristicPattern, CharacteristicPatternDto>, CharacteristicPatternDtoMapper>();
            services.AddScoped<ICategoryDtoMapper, CategoryDtoMapper>();
        }
    }
}
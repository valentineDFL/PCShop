using System.Runtime.CompilerServices;
using Domain.Dto.UserDtos;
using Domain.Interfaces.MappingW;
using Domain.Models;
using PCDetailsShop.API.DtoMapping;

namespace PCDetailsShop.API.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddDtoMapping(this IServiceCollection services)
        {
            services.AddScoped<IBaseMapper<User, UserDto>, UserResponseDtoMapper>();
        }
    }
}

using System.Runtime.CompilerServices;
using PCDetailsShop.API.DtoMapping;

namespace PCDetailsShop.API.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddDtoMapping(this IServiceCollection services)
        {
            services.AddScoped(typeof(ResponseDtoMapper));
        }
    }
}

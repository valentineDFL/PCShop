using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace PCDetailsShop.API.Permissions
{
    public class PermissionAuthorizationHandler 
        : AuthorizationHandler<PermissionsRequirment>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionsRequirment requirement)
        {
            Console.WriteLine("Я ВЫЗВАЛСЯ ПРИ АВТОРИЗАЦИИ");

            var userId = context.User.Claims.FirstOrDefault(
                c => c.Type == CustomClaims.UserId);

            if (userId is null || !Guid.TryParse(userId.Value, out var id))
                return;

            using(var scope = _serviceScopeFactory.CreateScope())
            {
                var permissionService = scope.ServiceProvider
                    .GetRequiredService<IUserRepository>();

                List<Domain.Enums.Permissions> userPermissions = await permissionService.GetPermissionsAsync(id);

                if (userPermissions.Intersect(requirement.Permissions).Any())
                    context.Succeed(requirement);
                else
                    context.Fail();
            }
        }
    }
}

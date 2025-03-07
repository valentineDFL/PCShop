using Domain.Enums;
using Domain.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;

namespace PCDetailsShop.API.Permissions
{
    public class PermissionsRequirment : IAuthorizationRequirement 
    {
        public PermissionsRequirment(Domain.Enums.Permissions[] permissions)
        {
            Permissions = permissions;
        }

        public Domain.Enums.Permissions[] Permissions { get; set; } = [];
    }
}
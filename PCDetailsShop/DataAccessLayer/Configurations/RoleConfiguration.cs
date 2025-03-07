using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Configurations
{
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasAlternateKey(r => r.Name);

            builder.HasMany(r => r.Permissions)
                .WithMany(p => p.Roles);

            var roles = GetRoles();

            builder.HasData(roles);
        }

        private List<Role> GetRoles()
        {
            var roles = Enum
                .GetValues<Roles>()
                .Select(r => new Role
                {
                    Id = (int)r,
                    Name = r.ToString()
                }).ToList();

            var permissions = Enum
                .GetValues<Permissions>()
                .Select(p => new Permission
                {
                   Id = (int)p,
                   Name = p.ToString()
                }).ToList();

            for (int index = 0; index < roles.Count; index++)
            {
                roles[index].Permissions.Add(permissions[index]);
            }

            return roles;
        }
    }
}

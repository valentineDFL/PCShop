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
    internal class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasAlternateKey(p => p.Name);

            //var permissions = Enum
            //    .GetValues<Permissions>()
            //    .Select(p => new Permission
            //    {
            //        Id = (int)p,
            //        Name = p.ToString()
            //    });

            //builder.HasData(permissions);
        }
    }
}

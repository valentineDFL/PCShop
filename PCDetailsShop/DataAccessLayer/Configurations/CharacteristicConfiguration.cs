using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    internal class CharacteristicConfiguration : IEntityTypeConfiguration<CharacteristicEntity>
    {
        public void Configure(EntityTypeBuilder<CharacteristicEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(32);

            builder.Property(x => x.Value)
                .HasMaxLength(32);
        }
    }
}

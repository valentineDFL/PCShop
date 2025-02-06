using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities.Characteristic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    internal class CharacteristicPatternConfiguration : IEntityTypeConfiguration<CharacteristicPatternEntity>
    {
        public void Configure(EntityTypeBuilder<CharacteristicPatternEntity> builder)
        {
            builder.HasKey(cp => cp.Id);

            builder.Property(cp => cp.Name)
                .IsRequired()
                .HasMaxLength(32);
        }
    }
}
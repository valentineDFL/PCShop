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
    internal class CharacteristicRealizationConfiguration : IEntityTypeConfiguration<CharacteristicRealizationEntity>
    {
        public void Configure(EntityTypeBuilder<CharacteristicRealizationEntity> builder)
        {
            builder.HasKey(cr => cr.Id);

            builder.Property(cr => cr.Value)
                .IsRequired()
                .HasMaxLength(32);

            builder.HasOne(cp => cp.CharacteristicPattern)
                .WithOne()
                .HasForeignKey<CharacteristicRealizationEntity>(cp => cp.CharacteristicPatternId);
        }
    }
}

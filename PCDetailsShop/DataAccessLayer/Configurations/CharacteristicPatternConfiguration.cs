using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    internal class CharacteristicPatternConfiguration : IEntityTypeConfiguration<CharacteristicPattern>
    {
        public void Configure(EntityTypeBuilder<CharacteristicPattern> builder)
        {
            builder.HasKey(cp => cp.Id);

            builder.Property(cp => cp.Name)
                .IsRequired()
                .HasMaxLength(32);

            builder.HasMany(cp => cp.Realizations)
                .WithOne(cr => cr.CharacteristicPattern);
        }
    }
}
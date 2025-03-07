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
    internal class CharacteristicRealizationConfiguration : IEntityTypeConfiguration<CharacteristicRealization>
    {
        public void Configure(EntityTypeBuilder<CharacteristicRealization> builder)
        {
            builder.HasKey(cr => cr.Id);

            builder.Property(cr => cr.Value)
                .IsRequired()
                .HasMaxLength(32);
        }
    }
}

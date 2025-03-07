using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(2048);

            builder.HasMany(p => p.CharacteristicsRealizations)
                .WithOne(cr => cr.Product)
                .HasForeignKey(cr => cr.ProductId);
        }
    }
}
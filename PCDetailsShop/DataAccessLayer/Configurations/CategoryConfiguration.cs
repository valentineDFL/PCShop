using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasAlternateKey(x => x.Name);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(32);

            builder.HasMany(c => c.Products)
                .WithMany(p => p.Categories);

            builder.HasMany(c => c.CharacteristicPatterns)
                .WithOne(cp => cp.Category)
                .HasForeignKey(cp => cp.CategoryId);
        }
    }
}

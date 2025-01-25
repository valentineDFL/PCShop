using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasAlternateKey(s => s.Login);
            builder.Property(p => p.Login)
                .HasMaxLength(32);

            builder.HasAlternateKey(s => s.Email);
            builder.Property(p => p.Email)
                .HasMaxLength(32);

            builder.Property(p => p.Password)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(p => p.BirthDate)
                .IsRequired();

            builder.HasOne(p => p.Cart)
                .WithOne(p => p.User)
                .HasForeignKey<CartEntity>(p => p.UserId);
        }
    }
}
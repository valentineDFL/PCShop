using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasAlternateKey(u => u.Login);
            builder.Property(u => u.Login)
                .HasMaxLength(32);

            builder.HasAlternateKey(u => u.Email);
            builder.Property(u => u.Email)
                .HasMaxLength(32);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(u => u.BirthDate)
                .IsRequired();

            builder.HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(p => p.UserId);

            builder.HasMany(u => u.Roles)
                .WithMany(r => r.Users);

            User admin = GetAdmin();

            builder.HasData(admin);
        }

        private User GetAdmin()
        {
            User admin = new User
            {
                
            };
        }
    }
}
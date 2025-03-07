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
    internal class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder) // в будущем обратить внимание на данную реализацию связи между корзиной и продуктом
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.User)
                .WithOne(p => p.Cart)
                .HasForeignKey<User>(p => p.CartId);

            builder.HasMany(p => p.Products)
                .WithMany();
        }
    }
}
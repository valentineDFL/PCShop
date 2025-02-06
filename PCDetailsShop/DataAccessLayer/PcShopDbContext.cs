using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using DataAccessLayer.Entities.Characteristic;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    internal class PcShopDbContext : DbContext
    {
        public PcShopDbContext(DbContextOptions<PcShopDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<CartEntity> Carts { get; set; }

        public DbSet<ProductEntity> Products { get; set; }

        public DbSet<CategoryEntity> Categories { get; set; }

        public DbSet<CharacteristicPatternEntity> CharacteristicPatterns { get; set; }

        public DbSet<CharacteristicRealizationEntity> CharacteristicRealizing { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
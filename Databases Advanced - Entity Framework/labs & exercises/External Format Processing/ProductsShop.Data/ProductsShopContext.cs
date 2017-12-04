using System;
using Microsoft.EntityFrameworkCore;
using ProductsShop.Models;
using ProductsShop.Data.EntityConfigs;

namespace ProductsShop.Data
{
    public class ProductsShopContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new ProductConfig());
            builder.ApplyConfiguration(new CategoryProductConfig());
            builder.ApplyConfiguration(new CategoryConfig());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)=>
            builder.UseSqlServer(ServerConfig.connectionString);
    }
}

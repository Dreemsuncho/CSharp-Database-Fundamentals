using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext() { }
        public SalesDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Store> Stores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(product =>
            {
                product.Property(p => p.Name)
                    .HasMaxLength(50)
                    .IsUnicode();

                product.Property(p => p.Price)
                    .HasColumnType("MONEY");
            });


            modelBuilder.Entity<Customer>(customer =>
            {
                customer.Property(c => c.Name)
                    .HasMaxLength(100)
                    .IsUnicode();

                customer.Property(c => c.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<Store>(store =>
            {
                store.Property(s => s.Name)
                    .HasMaxLength(80)
                    .IsUnicode(true);
            });


            modelBuilder.Entity<Sale>(sale =>
            {
                sale.HasOne<Product>()
                    .WithMany(p => p.Sales)
                    .HasForeignKey(s => s.ProductId);

                sale.HasOne<Customer>()
                    .WithMany(c => c.Sales)
                    .HasForeignKey(s => s.CustomerId);

                sale.HasOne<Store>()
                    .WithMany(s => s.Sales)
                    .HasForeignKey(s => s.StoreId);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(Configuration.connectionString);
        }
    }
}

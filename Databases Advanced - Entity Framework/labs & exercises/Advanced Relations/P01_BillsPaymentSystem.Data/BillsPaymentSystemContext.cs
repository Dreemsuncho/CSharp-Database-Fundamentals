using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.Data.EntityConfiguration;
using P01_BillsPaymentSystem.Models;

namespace P01_BillsPaymentSystem.Data
{
    public class BillsPaymentSystemContext : DbContext
    {
        public BillsPaymentSystemContext() { }
        public BillsPaymentSystemContext(DbContextOptions options) : base(options) { }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new CreditCardConfiguration());
            builder.ApplyConfiguration(new BankAccountConfiguration());
            builder.ApplyConfiguration(new PaymentMethodConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder) =>
            builder.UseSqlServer(Configuration.connectionString);
    }
}
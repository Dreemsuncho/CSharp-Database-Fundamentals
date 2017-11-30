using Microsoft.EntityFrameworkCore;
using EmployeeMapping.Models;

namespace EmployeeMapping.Data
{
    public class EmployeeContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(ServerConfig.connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Employee>(employee =>
            {
                employee.Property(e => e.FirstName)
                    .IsRequired();

                employee.Property(e => e.LastName)
                    .IsRequired();

                employee.Property(e => e.Salary)
                    .IsRequired();

                employee.HasOne(e => e.Manager)
                    .WithMany(m => m.Employees)
                    .HasForeignKey(e => e.ManagerId);
            });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using BusTicketSystem.Models;

using BusTicketSystem.Data.EntityConfig;

namespace BusTicketSystem.Data
{
    public class BusTicketContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<BusStation> BusStations { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<ArrivedTrip> ArrivedTrips { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfig());
            modelBuilder.ApplyConfiguration(new TicketConfig());
            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new TripConfig());
            modelBuilder.ApplyConfiguration(new BusStationConfig());
            modelBuilder.ApplyConfiguration(new TownConfig());
            modelBuilder.ApplyConfiguration(new ReviewConfig());
            modelBuilder.ApplyConfiguration(new BankAccountConfig());
            modelBuilder.ApplyConfiguration(new ArrivedTripConfig());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(SourceConfig._connectionString);
        }
    }
}

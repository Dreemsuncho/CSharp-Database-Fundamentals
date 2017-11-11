using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext() { }
        public HospitalDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Visitation> Visitations { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<PatientMedicament> PatientMedicaments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(Configuration.connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientMedicament>()
                .ToTable("PatientMedicament");


            modelBuilder.Entity<Patient>(patient =>
            {
                patient.Property(p => p.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode();

                patient.Property(p => p.LastName)
                    .HasMaxLength(50)
                    .IsUnicode();

                patient.Property(p => p.Address)
                    .HasMaxLength(50)
                    .IsUnicode();

                patient.Property(p => p.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<Visitation>(visitation =>
            {
                visitation.Property(v => v.Comments)
                    .HasMaxLength(250)
                    .IsUnicode();

                visitation.HasOne<Patient>()
                    .WithMany(p => p.Visitations)
                    .HasForeignKey(v => v.PatientId);

                visitation.HasOne<Doctor>()
                    .WithMany(d => d.Visitations)
                    .HasForeignKey(v => v.DoctorId);
            });


            modelBuilder.Entity<Diagnose>(diagnose =>
            {
                diagnose.Property(d => d.Name)
                    .HasMaxLength(50)
                    .IsUnicode();

                diagnose.Property(d => d.Comments)
                    .HasMaxLength(250)
                    .IsUnicode();

                diagnose.HasOne<Patient>()
                    .WithMany(p => p.Diagnoses)
                    .HasForeignKey(d => d.PatientId);
            });


            modelBuilder.Entity<Medicament>(medicament =>
            {
                medicament.Property(m => m.Name)
                    .HasMaxLength(50)
                    .IsUnicode();
            });


            modelBuilder.Entity<PatientMedicament>(patientMedicament =>
            {
                patientMedicament.HasKey(pm => new { pm.PatientId, pm.MedicamentId });

                patientMedicament.HasOne<Patient>()
                    .WithMany(p=>p.Prescriptions)
                    .HasForeignKey(pm => pm.PatientId);

                patientMedicament.HasOne<Medicament>()
                    .WithMany(m=>m.Prescriptions)
                    .HasForeignKey(pm => pm.MedicamentId);
            });
        }
    }
}

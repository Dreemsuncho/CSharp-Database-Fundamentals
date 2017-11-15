using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext() { }
        public StudentSystemContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Homework> HomeworkSubmissions { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Student>(student =>
            {
                student.Property(s => s.Name)
                    .HasMaxLength(100)
                    .IsUnicode(true);

                student.Property(s => s.PhoneNumber)
                    .HasColumnType("CHAR(10)")
                    .IsUnicode(false)
                    .IsRequired(false);

                student.Property(s => s.Birthday)
                    .IsRequired(false);

                student.HasMany(s => s.HomeworkSubmissions)
                    .WithOne(hs => hs.Student)
                    .HasForeignKey(hs => hs.StudentId);
            });

            builder.Entity<Course>(course =>
            {
                course.Property(c => c.Name)
                    .HasMaxLength(80)
                    .IsUnicode();

                course.Property(c => c.Description)
                    .IsUnicode()
                    .IsRequired(false);

                course.HasMany(c => c.Resources)
                    .WithOne(r => r.Course)
                    .HasForeignKey(r => r.CourseId);

                course.HasMany(c => c.HomeworkSubmissions)
                    .WithOne(hs => hs.Course)
                    .HasForeignKey(hs => hs.CourseId);
            });

            builder.Entity<Resource>(resource =>
            {
                resource.Property(r => r.Name)
                    .HasMaxLength(50)
                    .IsUnicode();

                resource.Property(r => r.Url)
                    .IsUnicode(false);
            });

            builder.Entity<Homework>(homework =>
            {
                homework.Property(h => h.Content)
                    .IsUnicode(false);
            });

            builder.Entity<StudentCourse>(studentCourse =>
            {
                studentCourse.HasKey(sc => new { sc.CourseId, sc.StudentId });

                studentCourse.HasOne(sc => sc.Course)
                    .WithMany(c => c.StudentsEnrolled)
                    .HasForeignKey(sc => sc.CourseId)
                    .OnDelete(DeleteBehavior.Restrict);

                studentCourse.HasOne(sc => sc.Student)
                    .WithMany(c => c.CourseEnrollments)
                    .HasForeignKey(sc => sc.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=.;Database=StudentSystem;Integrated Security=True");
        }
    }
}

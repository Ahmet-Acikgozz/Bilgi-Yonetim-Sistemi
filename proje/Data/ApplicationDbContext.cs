using Microsoft.EntityFrameworkCore;
using proje.Models.Entities;
namespace proje.Data { 
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentCourse> StudentCourses { get; set; }

    // ilişki yapılandırmalarını burada yapabiliriz
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Many-to-Many ilişkiyi tanımlama
        modelBuilder.Entity<StudentCourse>()
            .HasKey(sc => new { sc.Student_ID, sc.Course_ID });

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.StudentCourses)
            .HasForeignKey(sc => sc.Student_ID);

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.StudentCourses)
            .HasForeignKey(sc => sc.Course_ID);

        base.OnModelCreating(modelBuilder);
    }
}}
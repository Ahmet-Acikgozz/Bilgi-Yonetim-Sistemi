using Microsoft.EntityFrameworkCore;
using proje.Models.Entities;

namespace proje.Data
{
    //sınıfın özelliğini kullanıyoruz
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}

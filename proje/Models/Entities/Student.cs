using System.ComponentModel.DataAnnotations;

namespace proje.Models.Entities
{
    public class Student
    {
        [Key]
        public int Student_ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public
        string Email
        { get; set; }
        public string Password { get; set; }
        public string Major { get; set; }

        // Bir öğrencinin birden fazla dersi olabilir (bir-çok ilişkisi)
        public List<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}

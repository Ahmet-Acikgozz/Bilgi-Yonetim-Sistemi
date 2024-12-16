using System.ComponentModel.DataAnnotations;

namespace proje.Models.Entities
{
    public class Instructor
    {
        [Key]
        public int Instructor_ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public
        string Department
        { get; set; }

        // Bir akademisyen birden fazla ders verebilir (bir-çok ilişkisi)
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}

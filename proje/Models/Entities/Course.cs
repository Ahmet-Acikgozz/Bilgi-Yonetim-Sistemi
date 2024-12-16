using System.ComponentModel.DataAnnotations;

namespace proje.Models.Entities
{
    public class Course
    {
        [Key] //bir özelliğin (property'nin) sınıfın birincil anahtarı (Primary Key) olduğunu belirtir.
        public int Course_ID { get; set; } //property her biri
        public string Course_Name { get; set; }
        public int Credits { get; set; }

        // Bir ders bir akademisyen tarafından verilir (bir-bir ilişkisi)
        public int? Instructor_ID { get; set; }
        public Instructor Instructor { get; set; } // bir property'dir ve ilişkili eğitmen nesnesine erişim sağlar.


        // Bir dersi birden fazla öğrenci seçebilir (bir-çok ilişkisi)
        public List<Student> Students { get; set; } = new List<Student>(); //başlangıçta boş bir liste atanması için kullanılır. Null hatalarını önlemek için

    }
}

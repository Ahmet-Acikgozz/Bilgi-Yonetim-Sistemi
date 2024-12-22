using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proje.Data;
using proje.Models.Entities;

namespace proje.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Öğrenci Login Sayfası
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Öğrenci Login İşlemi
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var student = _context.Students
                .FirstOrDefault(s => s.Email == email && s.Password == password);

            if (student == null)
            {
                ViewBag.ErrorMessage = "Geçersiz kullanıcı adı veya şifre.";
                return View();
            }

            // Session Oluşturma
            HttpContext.Session.SetString("StudentEmail", student.Email);

            return RedirectToAction("Dashboard");
        }

        // Öğrenci Logout İşlemi
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("StudentEmail");
            return RedirectToAction("Login");
        }

        // Öğrenci Dashboard (Ders Seçimi ve Oturum Bilgisi)
        public IActionResult Dashboard()
        {
            var studentEmail = HttpContext.Session.GetString("StudentEmail");

            if (studentEmail == null)
            {
                return RedirectToAction("Login");
            }

            var student = _context.Students
                .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                .FirstOrDefault(s => s.Email == studentEmail);

            // Öğrenciye ait dersler ve onay durumlarını listele
            return View(student);
        }

        // Öğrenci Ders Seçme Sayfası (Sadece Onaylı Dersler)
        [HttpGet]
        public IActionResult SelectCourses()
        {
            var studentEmail = HttpContext.Session.GetString("StudentEmail");

            if (studentEmail == null)
            {
                return RedirectToAction("Login");
            }

            var student = _context.Students.FirstOrDefault(s => s.Email == studentEmail);
            if (student == null)
            {
                return RedirectToAction("Login");
            }

            // Öğrencinin daha önce seçmediği dersleri alıyoruz.
            // StudentCourse tablosunda öğrencinin seçtiği derslerle karşılaştırma yapıyoruz.
            var selectedCourseIds = _context.StudentCourses
                .Where(sc => sc.Student_ID == student.Student_ID)
                .Select(sc => sc.Course_ID) // Öğrencinin seçtiği derslerin ID'lerini al
                .ToList();

            // Tüm dersleri al ve öğrenci tarafından seçilmeyen dersleri filtrele
            var availableCourses = _context.Courses
                .Where(c => !selectedCourseIds.Contains(c.Course_ID)) // Öğrencinin seçmediği dersler
                .ToList();


            return View(availableCourses);
        }

        // Öğrenci Ders Seçme İşlemi
        [HttpPost]
        public IActionResult SelectCourses(int[] selectedCourseIds)
        {
            var studentEmail = HttpContext.Session.GetString("StudentEmail");

            if (studentEmail == null)
            {
                return RedirectToAction("Login");
            }

            var student = _context.Students
                .FirstOrDefault(s => s.Email == studentEmail);

            if (student == null)
            {
                return RedirectToAction("Login");
            }

            foreach (var courseId in selectedCourseIds)
            {
                var studentCourse = new StudentCourse
                {
                    Student_ID = student.Student_ID,
                    Course_ID = courseId,
                    IsApproved = false // Başlangıçta ders onaylı değil
                };

                _context.StudentCourses.Add(studentCourse);
            }

            _context.SaveChanges();

            TempData["Message"] = "Dersler başarıyla seçildi!";
            return RedirectToAction("Dashboard");
        }
        // Şifre Değiştirme Sayfası (GET)
        [HttpGet]
        public IActionResult ChangePassword()
        {
            var studentEmail = HttpContext.Session.GetString("StudentEmail");

            if (studentEmail == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        // Şifre Değiştirme İşlemi (POST)
        [HttpPost]
        public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            var studentEmail = HttpContext.Session.GetString("StudentEmail");
            if (studentEmail == null)
            {
                return RedirectToAction("Login");
            }

            var student = _context.Students.FirstOrDefault(s => s.Email == studentEmail);

            if (student == null)
            {
                return RedirectToAction("Login");
            }

            // Mevcut şifrenin doğruluğunu kontrol et
            if (student.Password != currentPassword)
            {
                ViewBag.ErrorMessage = "Mevcut şifre yanlış.";
                return View();
            }

            // Yeni şifre eski şifreyle aynı olmasın
            if (currentPassword == newPassword)
            {
                ViewBag.ErrorMessage = "Yeni şifre mevcut şifreyle aynı olamaz.";
                return View();
            }

            

            // Şifreyi güncelle
            student.Password = newPassword;
            _context.SaveChanges();

            TempData["Message"] = "Şifreniz başarıyla güncellendi!";
            return RedirectToAction("Dashboard");
        }
    }
}


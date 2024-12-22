using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proje.Data;
using proje.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace proje.Controllers
{
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstructorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Öğretmen giriş sayfası
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Öğretmen oturum açma işlemi
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Email ve şifre gereklidir.";
                return View();
            }

            var instructor = _context.Instructors
                .FirstOrDefault(i => i.Email == email && i.Password == password);

            if (instructor == null)
            {
                ViewBag.ErrorMessage = "Geçersiz kullanıcı adı veya şifre.";
                return View();
            }

            // Oturumu başlat
            HttpContext.Session.SetInt32("InstructorID", instructor.Instructor_ID);
            return RedirectToAction("Dashboard");
        }

        // Oturum kapatma (Logout)
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // Öğretmenin kendi derslerini gösteren dashboard
        [HttpGet]
        public IActionResult Dashboard()
        {
            var instructorID = HttpContext.Session.GetInt32("InstructorID");

            if (instructorID == null)
            {
                return RedirectToAction("Login");
            }

            var instructor = _context.Instructors
                .Include(i => i.Courses)
                .FirstOrDefault(i => i.Instructor_ID == instructorID);

            if (instructor == null)
            {
                return RedirectToAction("Login");
            }

            return View(instructor);
        }

        // Öğretmenin verdiği dersleri seçen öğrencileri listeleme
        [HttpGet]
        public IActionResult ViewStudents(int courseId)
        {
            var instructorID = HttpContext.Session.GetInt32("InstructorID");

            if (instructorID == null)
            {
                return RedirectToAction("Login");
            }

            var course = _context.Courses
                .Include(c => c.StudentCourses)
                .ThenInclude(sc => sc.Student)
                .FirstOrDefault(c => c.Course_ID == courseId && c.Instructor_ID == instructorID);

            if (course == null)
            {
                return RedirectToAction("Dashboard");
            }

            ViewBag.ApprovedStudents = course.StudentCourses
                .Where(sc => sc.IsApproved)
                .Select(sc => sc.Student)
                .ToList();

            ViewBag.PendingStudents = course.StudentCourses
                .Where(sc => !sc.IsApproved)
                .Select(sc => sc.Student)
                .ToList();

            ViewBag.CourseId = courseId;

            return View();
        }

        // Öğretmen tarafından ders onayı verme
        [HttpPost]
        public IActionResult ApproveCourse(int studentId, int courseId)
        {
            var instructorID = HttpContext.Session.GetInt32("InstructorID");

            if (instructorID == null)
            {
                return RedirectToAction("Login");
            }

            var studentCourse = _context.StudentCourses
                .FirstOrDefault(sc => sc.Student_ID == studentId && sc.Course_ID == courseId);

            if (studentCourse != null)
            {
                studentCourse.IsApproved = true;
                _context.SaveChanges();
            }

            return RedirectToAction("ViewStudents", new { courseId = courseId });
        }

        // Şifre Değiştirme İşlemi (GET)
        [HttpGet]
        public IActionResult ChangePassword()
        {
            var instructorID = HttpContext.Session.GetInt32("InstructorID");

            if (instructorID == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        // Şifre Değiştirme İşlemi (POST)
        [HttpPost]
        public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            var instructorID = HttpContext.Session.GetInt32("InstructorID");

            if (instructorID == null)
            {
                return RedirectToAction("Login");
            }

            var instructor = _context.Instructors.FirstOrDefault(i => i.Instructor_ID == instructorID);

            if (instructor == null)
            {
                return RedirectToAction("Login");
            }

            // Mevcut şifrenin doğruluğunu kontrol et
            if (instructor.Password != currentPassword)
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

            // Yeni şifre ile onay şifresi eşleşiyor mu kontrol et
            if (newPassword != confirmPassword)
            {
                ViewBag.ErrorMessage = "Yeni şifre ve onay şifresi eşleşmiyor.";
                return View();
            }

            // Şifreyi güncelle
            instructor.Password = newPassword;
            _context.SaveChanges();

            TempData["Message"] = "Şifreniz başarıyla güncellendi!";
            return RedirectToAction("Dashboard");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using proje.Data;

namespace proje.Controllers
{
    public class InstructorLoginController : Controller
    {
        // Veritabanı erişimi için gerekli context
        private readonly ApplicationDbContext _context;

        // Constructor - veritabanını kullanmamızı sağlar
        public InstructorLoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Login Sayfasını Döndürür (GET Metodu)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Login İşlemini Gerçekleştirir (POST Metodu)
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
                ViewBag.ErrorMessage = "Geçersiz email veya şifre.";
                return View();
            }

            // Instructor Controller'daki Index'e yönlendirir
            return RedirectToAction("Index", "Instructor");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using proje.Data;

namespace proje.Controllers
{
    public class LoginController : Controller
    {
        //veritabanını kullanmamızı sağlar
        private readonly ApplicationDbContext _context;

        // Constructor ilk veritabanındaki bilgilere erişmemizi sağlar
        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Email ve şifre gereklidir.";
                return View();
            }

            var student = _context.Students
                .FirstOrDefault(s => s.Email == email && s.Password == password);

            if (student == null)
            {
                ViewBag.ErrorMessage = "Geçersiz kullanıcı adı veya şifre.";
                return View();
            }
            // student sayfasına yönlendirir
            return RedirectToAction("Index", "Students");
        }
    }
}

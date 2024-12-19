using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị form đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Xử lý đăng ký
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên đăng nhập đã tồn tại chưa
                if (_context.Users.Any(u => u.Username == user.Username))
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại.");
                    return View(user);
                }

                // Kiểm tra xem email đã tồn tại chưa
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");
                    return View(user);
                }

                // Lưu người dùng vào cơ sở dữ liệu
                _context.Users.Add(user);
                _context.SaveChanges();

                // Tự động đăng nhập sau khi đăng ký
                HttpContext.Session.SetString("Username", user.Username);

                // Chuyển hướng đến danh sách sản phẩm
                return RedirectToAction("Index", "Product");
            }

            return View(user);
        }

        // Hiển thị form đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                // Nếu người dùng đã đăng nhập, chuyển hướng đến trang sản phẩm
                return RedirectToAction("Index", "Product");
            }
            return View();
        }

        // Xử lý đăng nhập
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.";
                return View();
            }

            // Tìm người dùng trong cơ sở dữ liệu
            var user = _context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                // Lưu thông tin đăng nhập vào session
                HttpContext.Session.SetString("Username", user.Username);

                // Chuyển hướng đến danh sách sản phẩm
                return RedirectToAction("Index", "Product");
            }

            // Nếu đăng nhập thất bại, hiển thị thông báo lỗi
            ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không chính xác.";
            return View();
        }

        // Đăng xuất
        public IActionResult Logout()
        {
            // Xóa session
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TT_ECommerce.Data;
using TT_ECommerce.Models;

namespace TT_ECommerce.Controllers
{
    public class LoginAdminController : Controller
    {
        private readonly TT_ECommerceDbContext _context;

        public LoginAdminController(TT_ECommerceDbContext context)
        {
            _context = context;
        }

        // GET: LoginAdmin
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string Acc, string Pass)
        {
            if (ModelState.IsValid)
            {
                // Tìm admin trong cơ sở dữ liệu
                var userAdmin = await _context.UserAdmins
                    .FirstOrDefaultAsync(u => u.Username.ToLower() == Acc.ToLower() && u.IsActive);

                // Kiểm tra xem admin có tồn tại và mật khẩu có khớp không
                if (userAdmin != null && userAdmin.Password == Pass) // Bỏ kiểm tra mã hóa
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userAdmin.Username),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Đăng nhập
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng.");
                }
            }

            return View();
        }

        // Phương thức để đăng xuất
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "LoginAdmin"); // Chuyển hướng về trang đăng nhập
        }
    }
}

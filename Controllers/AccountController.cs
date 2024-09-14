using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TT_ECommerce.Models;
using System.Threading.Tasks;
using TT_ECommerce.Services;

namespace TT_ECommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly OtpService _otpService; // Dịch vụ OTP
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, OtpService otpService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _otpService = otpService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    // Tạo và gửi OTP
                    var otp = _otpService.GenerateOtp();
                    await _otpService.SendOtpAsync(model.Email, otp);

                    // Lưu OTP vào session
                    HttpContext.Session.SetString("OtpEmail", model.Email);
                    HttpContext.Session.SetString("Otp", otp);

                    Console.WriteLine($"User {model.Email} signed in successfully. OTP generated and sent: {otp}");

                    return RedirectToAction("VerifyOtp", new { email = model.Email });
                }
                if (result.IsLockedOut)
                {
                    return RedirectToAction("Lockout");
                }
                else
                {
                    Console.WriteLine($"Invalid login attempt for user {model.Email}.");
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            Console.WriteLine("Model state is invalid.");
            return View(model);
        }

        [HttpGet]
        public IActionResult VerifyOtp(string email)
        {
            // Hiển thị trang xác thực OTP
            return View(new VerifyOtpViewModel { Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyOtp(VerifyOtpViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Lấy OTP và email từ session
                var storedOtp = HttpContext.Session.GetString("Otp");
                var email = HttpContext.Session.GetString("OtpEmail");

                Console.WriteLine($"Verifying OTP for email: {email}. OTP entered: {model.Otp}");
                Console.WriteLine($"Stored OTP for email: {email} is {storedOtp}");

                if (storedOtp != null && email != null && storedOtp == model.Otp)
                {
                    Console.WriteLine($"OTP verification succeeded for email: {email}.");

                    // Xóa OTP khỏi session sau khi xác thực thành công
                    HttpContext.Session.Remove("Otp");
                    HttpContext.Session.Remove("OtpEmail");

                    // OTP hợp lệ, tiếp tục đăng nhập
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Console.WriteLine($"Invalid OTP provided for email: {email}.");
                    ModelState.AddModelError(string.Empty, "Invalid OTP.");
                }
            }
            else
            {
                Console.WriteLine("Model state is invalid during OTP verification.");
            }

            return View(model);
        }


        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Gán vai trò 'USER' cho người dùng sau khi đăng ký thành công
                    await _userManager.AddToRoleAsync(user, "USER");

                    // Đăng nhập người dùng ngay sau khi đăng ký thành công
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Ghi ra console khi đăng ký thành công
                    Console.WriteLine("Đăng ký thành công cho người dùng: " + user.Email);

                    return RedirectToAction("Index", "Home");
                }

                // Nếu có lỗi trong quá trình đăng ký, hiển thị lỗi
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Nếu model không hợp lệ, trả về trang đăng ký với các lỗi
            return View(model);
        }

        // GET: /Account/Manage
        [HttpGet]
        public IActionResult Manage()
        {
            // Đưa thông tin người dùng đến view
            return View();
        }
    }
}

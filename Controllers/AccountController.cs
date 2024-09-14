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
        private static string _generatedOtp; // Lưu mã OTP tạm thời
        private static string _otpUserEmail; // Lưu email người dùng
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, OtpService otpService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _otpService = otpService;
        }

         // GET: /Account/Login
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
                    // Nếu đăng nhập email và mật khẩu thành công, tạo mã OTP và gửi email
                    _generatedOtp = _otpService.GenerateOtp();
                    _otpUserEmail = model.Email; // Lưu email để dùng sau khi xác thực OTP

                    // Gửi mã OTP qua email
                    await _otpService.SendOtpAsync(model.Email, _generatedOtp);

                    // Điều hướng đến trang nhập OTP
                    return RedirectToAction("VerifyOtp");
                }
                if (result.IsLockedOut)
                {
                    return RedirectToAction("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
        }

        // GET: /Account/VerifyOtp
        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }

        // POST: /Account/VerifyOtp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOtp(string otpInput)
        {
            if (otpInput == _generatedOtp) // Kiểm tra mã OTP người dùng nhập
            {
                // OTP chính xác, người dùng đã được xác thực
                await _signInManager.SignInAsync(await _userManager.FindByEmailAsync(_otpUserEmail), isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid OTP.");
                return View();
            }
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

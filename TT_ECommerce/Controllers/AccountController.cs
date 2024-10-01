using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TT_ECommerce.Models;
using System.Threading.Tasks;
using TT_ECommerce.Services;
using static System.Net.WebRequestMethods;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly OtpService _otpService;
        private readonly EmailService _emailService;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, OtpService otpService, EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _otpService = otpService;
            _emailService = emailService;
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
                IdentityUser user;
                string userEmail = string.Empty;  // Khởi tạo biến để lưu email người dùng

                // Kiểm tra xem chuỗi nhập vào có phải email hợp lệ không
                if (new EmailAddressAttribute().IsValid(model.UsernameOrEmail))
                {
                    // Nếu là email, tìm người dùng theo email
                    user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
                    userEmail = model.UsernameOrEmail;  // Lưu email từ input
                }
                else
                {
                    // Nếu không, tìm người dùng theo username
                    user = await _userManager.FindByNameAsync(model.UsernameOrEmail);

                    // Nếu tìm thấy user, lấy email từ tài khoản của họ
                    if (user != null)
                    {
                        userEmail = user.Email;  // Lưu email từ tài khoản người dùng
                    }
                }

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                        var otp = _otpService.GenerateOtp();
                        try
                        {
                            // Gửi OTP đến email đã xác định trước
                            await _emailService.SendEmailAsync(userEmail, "OTP Verification", $"Your OTP is: {otp}");

                            HttpContext.Session.SetString("OtpEmail", userEmail);  // Lưu email người dùng vào session
                            HttpContext.Session.SetString("Otp", otp);

                            Console.WriteLine($"User {userEmail} signed in successfully. OTP generated and sent");
                            return RedirectToAction("VerifyOtp", new { email = userEmail });
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error sending OTP email: {ex.Message}");
                            ModelState.AddModelError(string.Empty, "Error sending OTP email. Please try again later.");
                            return View(model);
                        }
                    }

                    if (result.IsLockedOut)
                    {
                        return RedirectToAction("Lockout");
                    }
                    else
                    {
                        Console.WriteLine($"Invalid login attempt for user {user.UserName}.");
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

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
        [HttpGet]
        public IActionResult RegistrationConfirmation()
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
                // Sử dụng UserName là model.UserName và Email là model.Email
                var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Tạo token xác thực email
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                    try
                    {
                        // Gửi email xác thực
                        await _emailService.SendEmailAsync(model.Email, "Confirm your email",
                            $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>link</a>");

                        // Gán vai trò 'USER' cho người dùng sau khi gửi email xác thực
                        await _userManager.AddToRoleAsync(user, "USER");

                        // Đăng nhập người dùng ngay sau khi đăng ký thành công
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        Console.WriteLine("Đăng ký thành công cho người dùng: " + user.Email);

                        // Chuyển hướng đến trang xác nhận đăng ký
                        return RedirectToAction("RegistrationConfirmation");
                    }
                    catch (Exception ex)
                    {
                        // Log lỗi nếu không gửi được email
                        Console.WriteLine($"Error sending email: {ex.Message}");
                        ModelState.AddModelError(string.Empty, "Có lỗi khi gửi email xác thực. Vui lòng thử lại sau.");
                        return View(model);
                    }
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


        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }

            return View("Error");
        }
        // GET: Forgot Password
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Forgot Password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tìm người dùng dựa trên email
                var user = await _userManager.FindByEmailAsync(model.Email);

                // Nếu người dùng không tồn tại
                if (user == null)
                {
                    // Thông báo lỗi email không tồn tại
                    ModelState.AddModelError(string.Empty, "Email không tồn tại trong hệ thống.");
                    return View(model);
                }
                // Nếu email của người dùng chưa được xác nhận
                else if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    // Thông báo yêu cầu xác nhận email
                    ModelState.AddModelError(string.Empty, "Email chưa được xác nhận. Vui lòng kiểm tra hộp thư để xác nhận email.");
                    return View(model);
                }

                // Tạo token và link reset mật khẩu
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.Action("ResetPassword", "Account", new { token, email = model.Email }, Request.Scheme);

                try
                {
                    // Gửi email reset mật khẩu
                    await _emailService.SendEmailAsync(model.Email, "Reset Password", $"Click here to reset your password: <a href='{resetLink}'>link</a>");

                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    // Log lỗi nếu không gửi được email
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Có lỗi khi gửi email reset mật khẩu. Vui lòng thử lại sau.");
                    return View(model);
                }

                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        // GET: Forgot Password Confirmation
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: Reset Password
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new ResetPasswordViewModel { Token = token, Email = email });
        }

        // POST: Reset Password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        // GET: Reset Password Confirmation
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Get the logged-in user's username or email from the Identity
            var usernameOrEmail = User.Identity?.Name;
            if (usernameOrEmail == null)
            {
                return RedirectToAction("Login");
            }

            IdentityUser user;

            // Kiểm tra xem usernameOrEmail có phải là email hợp lệ không
            if (new EmailAddressAttribute().IsValid(usernameOrEmail))
            {
                // Nếu là email, tìm người dùng theo email
                user = await _userManager.FindByEmailAsync(usernameOrEmail);
            }
            else
            {
                // Nếu là username, tìm người dùng theo username
                user = await _userManager.FindByNameAsync(usernameOrEmail);
            }

            if (user == null)
            {
                // Nếu không tìm thấy người dùng, chuyển hướng về trang đăng nhập
                return RedirectToAction("Login");
            }

            // Chuẩn bị dữ liệu cho ViewModel UserProfile
            var model = new UserProfileViewModel
            {
                Email = user.Email,
                UserName = user.UserName,
                // Add other properties as needed (e.g., FirstName, LastName, etc.)
            };

            return View(model);
        }

        // GET: /Account/Logout
        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Đăng xuất người dùng
            await _signInManager.SignOutAsync();

            // Chuyển hướng đến trang chính sau khi đăng xuất
            return RedirectToAction("Index", "Home");
        }



    }
}

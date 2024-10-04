using Microsoft.AspNetCore.Mvc;

namespace TT_ECommerce.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View("Register");
        }
        public IActionResult Login()
        {
            return View("Login");
        }
        public IActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }
        public IActionResult ForgotPasswordConfirmation()
        {
            return View("ForgotPasswordConfirmation");
        }
        public IActionResult Manage()
        {
            return View("Manage");
        }
        public IActionResult Profile()
        {
            return View("Profile");
        }
        public IActionResult RegistrationConfirmation()
        {
            return View("RegistrationConfirmation");
        }
        public IActionResult ResetPassword()
        {
            return View("ResetPassword");
        }
    }
}

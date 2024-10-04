using Microsoft.AspNetCore.Mvc;

namespace TT_ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")] // Correct attribute for areas
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
     
    }
}

using Microsoft.AspNetCore.Mvc;

namespace TT_ECommerce.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

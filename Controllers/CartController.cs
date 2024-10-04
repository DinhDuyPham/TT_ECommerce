using Microsoft.AspNetCore.Mvc;

namespace TT_ECommerce.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

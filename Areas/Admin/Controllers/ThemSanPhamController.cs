using Microsoft.AspNetCore.Mvc;
using TT_ECommerce.Data;

namespace TT_ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")] // Correct attribute for areas
    public class ThemSanPhamController : Controller
    {
        private readonly TT_ECommerceDbContext _context;
        public IActionResult Index()
        {
            return View();
        }
    }
}

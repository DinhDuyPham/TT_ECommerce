using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TT_ECommerce.Models;
using TT_ECommerce.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TT_ECommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TT_ECommerceDbContext _context; // Khai báo DbContext

        // Inject cả ILogger và DbContext vào constructor
        public HomeController(ILogger<HomeController> logger, TT_ECommerceDbContext context)
        {
            _logger = logger;
            _context = context; // Khởi tạo DbContext
            ViewBag.CartItemCount = GetCartItemCount(); // Gọi GetCartItemCount ở đây
        }

        // Hàm tính tổng số lượng sản phẩm trong giỏ hàng
        private int GetCartItemCount()
        {
            // Kiểm tra nếu _context hoặc DbSet là null
            if (_context == null || _context.TbOrderDetails == null)
            {
                return 0; // Trả về 0 nếu _context hoặc TbOrderDetails không tồn tại
            }

            // Tính tổng số lượng sản phẩm trong giỏ hàng
            var cartItemCount = _context.TbOrderDetails.Sum(od => od.Quantity);
            return cartItemCount;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

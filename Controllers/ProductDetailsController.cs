using Microsoft.AspNetCore.Mvc;
using TT_ECommerce.Models; // Import namespace chứa model Product
using System.Linq;
using TT_ECommerce.Data;

namespace TT_ECommerce.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly TT_ECommerceDbContext _context; // ApplicationDbContext là DbContext của bạn

        public ProductDetailsController(TT_ECommerceDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            // Lấy sản phẩm theo id từ cơ sở dữ liệu
            var product = _context.TbProducts.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            // Truyền product vào view
            return View(product);
        }
    }
}

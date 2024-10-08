using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TT_ECommerce.Data;

namespace TT_ECommerce.Components
{
    public class BestSellerViewComponent : ViewComponent
    {
        private readonly TT_ECommerceDbContext _context;

        public BestSellerViewComponent(TT_ECommerceDbContext context)
        {
            _context = context;
        }

        // Phương thức chính của View Component
        public IViewComponentResult Invoke(int topCount = 10)
        {
            var bestSellers = _context.TbOrderDetails
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalSold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(g => g.TotalSold)
                .Take(topCount)
                .ToList();

            // Lấy chi tiết sản phẩm từ bảng Products
            var bestSellerProducts = _context.TbProducts
                .Where(p => bestSellers.Select(bs => bs.ProductId).Contains(p.Id))
                .ToList();

            return View(bestSellerProducts);
        }
    }
}

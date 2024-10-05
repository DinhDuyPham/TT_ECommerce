using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TT_ECommerce.Data;
using TT_ECommerce.Models.EF;

namespace TT_ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductManagerController : Controller
    {
        private readonly TT_ECommerceDbContext _context;

        public ProductManagerController(TT_ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            // Truy vấn ban đầu cho sản phẩm
            var productsQuery = _context.TbProducts.Include(t => t.ProductCategory);

            // Đếm tổng số sản phẩm
            var totalItems = await productsQuery.CountAsync();

            // Phân trang với Skip và Take
            var products = await productsQuery
                .Skip((page - 1) * pageSize)  // Bỏ qua số sản phẩm của các trang trước
                .Take(pageSize)  // Lấy số sản phẩm trên trang hiện tại
                .ToListAsync();

            // Truyền các thông tin cần thiết cho ViewBag
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;

            // Trả về view với danh sách sản phẩm đã phân trang
            return View(products);
        }

        // GET: Admin/ProductManager/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbProduct = await _context.TbProducts
                .Include(t => t.ProductCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbProduct == null)
            {
                return NotFound();
            }

            return View(tbProduct);
        }

    }
}

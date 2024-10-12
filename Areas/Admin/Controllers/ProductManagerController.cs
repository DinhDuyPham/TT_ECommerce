using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        // GET: Admin/ProductManager
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            var productsQuery = _context.TbProducts.Include(t => t.ProductCategory);
            var totalItems = await productsQuery.CountAsync();
            var products = await productsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;

            return View(products);
        }

        // GET: Admin/ProductManager/CreateProduct
        [HttpGet]
        public IActionResult CreateProduct()
        {
            ViewBag.ProductCategories = _context.TbProductCategories.ToList();
            return View();
        }

        // POST: Admin/ProductManager/CreateProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(TbProduct pro, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgProducts");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Kiểm tra nếu có tệp hình ảnh được tải lên
                if (Image != null && Image.Length > 0)
                {
                    // Tạo tên tệp ngẫu nhiên để tránh trùng lặp
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    string filePath = Path.Combine(uploadPath, fileName);

                    // Lưu tệp vào đường dẫn chỉ định
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(fileStream);
                    }

                    // Lưu đường dẫn tương đối vào cơ sở dữ liệu
                    pro.Image = "/imgProducts/" + fileName;
                }

                // Thiết lập thời gian tạo và sửa đổi
                pro.CreatedDate = DateTime.Now;
                pro.ModifiedDate = DateTime.Now;

                // Thêm sản phẩm vào cơ sở dữ liệu
                _context.TbProducts.Add(pro);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            // Nếu có lỗi trong ModelState, truyền lại danh sách danh mục
            ViewBag.ProductCategories = _context.TbProductCategories.ToList();
            return View(pro);
        }
    }
}
